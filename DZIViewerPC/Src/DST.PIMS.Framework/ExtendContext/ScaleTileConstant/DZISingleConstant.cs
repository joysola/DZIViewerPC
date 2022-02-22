using CS_AES_CTR;
using DST.Common.Helper;
using DST.Database.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ExtendContext
{

    public class DZISingleConstant : IDZIConstant
    {
        public static DZISingleConstant Cons { get; } = new DZISingleConstant();
        /// <summary>
        /// 导航图片文件夹名称
        /// </summary>
        public string NavImgLevel { get; } = "8";
        /// <summary>
        /// 导航图片名称
        /// </summary>
        public string NavImgName { get; } = "0_0.jpg";
        /// <summary>
        /// 瓦片大小
        /// </summary>
        public int DZIImgSzie { get; } = 1024;
        /// <summary>
        /// 总宽度
        /// </summary>
        public double DZIImgMaxHeight { get; } = Math.Pow(2, 16);
        /// <summary>
        /// 总高度
        /// </summary>
        public double DZIImgMaxWidth { get; } = Math.Pow(2, 16);
        /// <summary>
        /// 最小层数显示
        /// </summary>
        public int DZIMinLevel { get; } = 6;

        public string SlideFile { get; } = "Slide.ini";


        public bool IsSlice(string baseDir)
        {
            var result = false;
            if (File.Exists(baseDir))
            {
                result = new FileInfo(baseDir).Extension == ".dst";
            }
            return result;
        }


        /// <summary>
        /// 获取瓦片路径
        /// </summary>
        /// <param name="tileLevel"></param>
        /// <param name="tilePositionX"></param>
        /// <param name="tilePositionY"></param>
        /// <returns></returns>
        public string GetTilePath(int tileLevel, int tilePositionX, int tilePositionY) => $"{tileLevel}/{tilePositionX}_{tilePositionY}.jpg";


        /// <summary>
        /// 获取导航图片
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public async Task<byte[]> GetNavImg(DZIModel model)
        {
            byte[] result = null;
            if (model.TileInfo.TryGetValue($"{NavImgLevel}/{NavImgName}", out DZITileInfo info))
            {
                result = new byte[info.Length];
                using var fs = new FileStream(model.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024, true);
                fs.Seek(279 + model.TitleDataLength + info.Postion, SeekOrigin.Begin);
                await fs.ReadAsync(result, 0, info.Length).ConfigureAwait(false);
                //using (var fs2 = new FileStream("xxx.jpg", FileMode.OpenOrCreate))
                //{
                //    fs2.Write(result, 0, result.Length);
                //}
            }
            return result;
        }

        public async Task<byte[]> GetTileData(Stream stream, DZIModel model, string key)
        {
            byte[] result = null;
            if (model.TileInfo.TryGetValue(key, out DZITileInfo tileInfo))
            {
                result = new byte[tileInfo.Length];
                stream.Seek(279 + model.TitleDataLength + tileInfo.Postion, SeekOrigin.Begin);
                await stream.ReadAsync(result, 0, tileInfo.Length).ConfigureAwait(false);
            }
            return result;
        }


        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<DZIModel> LoadSingleFile(string url)
        {
            if (!IsSlice(url))
            {
                return null;
            }

            var model = new DZIModel { FilePath = url };
            using (var fs = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024, true))
            {
                // version
                byte[] titleBytes = new byte[7];
                await fs.ReadAsync(titleBytes, 0, 7).ConfigureAwait(false);
                var version = Encoding.ASCII.GetString(titleBytes);
                model.Version = version;

                // titleTotalLength 数据索引文件头
                byte[] titleLengthtbytes = new byte[128];
                await fs.ReadAsync(titleLengthtbytes, 0, 128).ConfigureAwait(false);

                var titleHex = StringHelper.GetHexStringFromBytes(titleLengthtbytes);

                var off1 = titleHex.Substring(77, 2);
                var off2 = titleHex.Substring(235, 2);
                var off3 = titleHex.Substring(43, 2);
                var off4 = titleHex.Substring(122, 2);

                var offa = int.Parse(off1, NumberStyles.HexNumber);
                var offb = int.Parse(off2, NumberStyles.HexNumber);
                var offc = int.Parse(off3, NumberStyles.HexNumber);
                var offd = int.Parse(off4, NumberStyles.HexNumber);
                var offest = $"{offa}{offb}{offc}{offd}";
                var titleDataLength = int.Parse(offest); // 数据的文件头最终长度
                model.TitleDataLength = titleDataLength;

                // key
                byte[] tmpKeyBytes = new byte[128];
                await fs.ReadAsync(tmpKeyBytes, 0, 128).ConfigureAwait(false);

                var keyHex = StringHelper.GetHexStringFromBytes(tmpKeyBytes);
                var key1 = keyHex.Substring(103, 2);
                var key2 = keyHex.Substring(25, 2);
                var key3 = keyHex.Substring(77, 2);
                var key4 = keyHex.Substring(35, 2);
                var key5 = keyHex.Substring(188, 2);
                var key6 = keyHex.Substring(116, 2);
                var key7 = keyHex.Substring(229, 2);
                var key8 = keyHex.Substring(57, 2);
                var key = $"{key1}{key2}{key3}{key4}{key5}{key6}{key7}{key8}";
                var keyBytes = Encoding.ASCII.GetBytes(key);

                // keyCount
                byte[] keyCountBytes = new byte[16];
                await fs.ReadAsync(keyCountBytes, 0, 16).ConfigureAwait(false);
                //var keyCountStr = Encoding.ASCII.GetString(keyCountBytes);

                // 数据索引文件头
                byte[] dataInfoBytes = new byte[titleDataLength];
                await fs.ReadAsync(dataInfoBytes, 0, titleDataLength).ConfigureAwait(false);

                // ctr解密
                using var outputStream = new MemoryStream();
                var aes = new AES_CTR(keyBytes, keyCountBytes);
                await aes.DecryptStreamAsync(outputStream, new MemoryStream(dataInfoBytes));
                using (var sr = new StreamReader(outputStream))
                {
                    sr.BaseStream.Seek(0, SeekOrigin.Begin); // 流重新定位读取点
                    var data = await sr.ReadToEndAsync().ConfigureAwait(false);
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                    foreach (var keyValue in dict)
                    {
                        var strArry = keyValue.Value.Split(','); // value 是aa,bb形式的字符串
                        if (strArry?.Length == 2 && int.TryParse(strArry[0], out int postion) && int.TryParse(strArry[1], out int length))
                        {
                            var dziInfo = new DZITileInfo { Length = length, Postion = postion };
                            model.TileInfo.Add(keyValue.Key, dziInfo);
                        }
                    }
                }

            }


            return model;
            //var xx = dict["9/0_0.jpg"];

            //fs.Seek(279 + totalTitleLength + yy[0], SeekOrigin.Begin);
            //var bytes = new byte[yy[1]];
            //await fs.ReadAsync(bytes, 0, yy[1]);
            //using var fs2 = new FileStream("0_0.jpg", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 1024 * 1024, true);
            //await fs2.WriteAsync(bytes, 0, bytes.Length);

        }
    }
}
