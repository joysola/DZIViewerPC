using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Nico.TransDat
{
    internal class TransHelper
    {
        /// <summary>
        /// 读取图片转换成二进制数组
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns>二进制数组</returns>
        public static byte[] ReadImage(string imagePath)
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, System.IO.FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中
                    return imgBytesIn;
                }
            }
        }

        public static byte[] SetImageBytes(Bitmap image)
        {
            byte[] bt = null;
            using (MemoryStream mostream = new MemoryStream())
            {
                Bitmap bmp = new Bitmap(image);
                bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Jpeg);//将图像以指定的格式存入缓存内存流
                bt = new byte[mostream.Length];
                mostream.Position = 0;//设置留的初始位置
                mostream.Read(bt, 0, Convert.ToInt32(bt.Length));
            }
            return bt;
        }

        /// <summary>
        /// 将二进制写入dat文件
        /// </summary>
        /// <param name="fileName">dat文件名称</param>
        /// <param name="imgByte">二进制数组</param>
        public static void WriteByte(string fileName, byte[] imgByte)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                BinaryWriter binWriter = new BinaryWriter(fs);
                binWriter.Write(imgByte, 0, imgByte.Length);
                binWriter.Close();
                fs.Close();
                imgByte = null;
            }
        }

        /// <summary>
        /// 读取文件，转换成二进制
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>二进制</returns>
        public static byte[] ReadByteFile(string fileName)
        {
            byte[] bBuffer;
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryReader binReader = new BinaryReader(fs);
                bBuffer = new byte[fs.Length];
                binReader.Read(bBuffer, 0, (int)fs.Length);
                binReader.Close();
                fs.Close();
            }

            return bBuffer;
        }

        /// <summary>
        /// 二进制转换成图片
        /// </summary>
        public static Bitmap SetByteToImage(byte[] mybyte, string targetPath = null)
        {
            using (MemoryStream ms = new MemoryStream(mybyte))
            {
                System.Drawing.Image outputImg = System.Drawing.Image.FromStream(ms);
                if (!string.IsNullOrEmpty(targetPath))
                {
                    outputImg.Save(targetPath);
                }

                return (Bitmap)outputImg;
            }
        }

        /// <summary>
        /// 二进制转文件
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <param name="fullFileName">文件的完整路径，包含后缀等信息</param>
        public static void WriteFileFromByte(byte[] data, string fullFileName)
        {
            using (FileStream fs = new FileStream(fullFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(data, 0, data.Length);
            }

            data = null;
        }

        /// <summary>
        /// 二进制转换成图片
        /// </summary>
        public static void SetByteToImageNoReturn(byte[] mybyte, string targetPath = null)
        {
            using (MemoryStream ms = new MemoryStream(mybyte))
            {
                System.Drawing.Image outputImg = System.Drawing.Image.FromStream(ms);
                if (!string.IsNullOrEmpty(targetPath))
                {
                    outputImg.Save(targetPath);
                }

                outputImg.Dispose();
                outputImg = null;
                mybyte = null;
            }
        }

        /// <summary>
        /// 字符串转二进制
        /// </summary>
        public static byte[] StringToByte(String s)
        {
            return (new UnicodeEncoding()).GetBytes(s);
        }

        /// <summary>
        /// 二进制转字符串
        /// </summary>
        public static string ByteToString(byte[] buffer)
        {
            string res = (new UnicodeEncoding()).GetString(buffer, 0, buffer.Length);
            return res;
        }

        /// <summary>
        /// 字符串转换成Base64二进制
        /// </summary>
        public static byte[] StringToBase64StringByte(string sourceStr)
        {
            byte[] buffer = System.Text.Encoding.Default.GetBytes(sourceStr);
            string tmpStr = Convert.ToBase64String(buffer);
            buffer = null;
            sourceStr = null;
            return StringToByte(tmpStr);
        }

        /// <summary>
        /// Base64的二进制转换成string
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public static string Base64StringByteToString(byte[] buffer)
        {
            string str = ByteToString(buffer);
            str = str.Replace("\0", "");
            byte[] tmpBuf = Convert.FromBase64String(str);
            string res = System.Text.Encoding.Default.GetString(tmpBuf);
            return res;
        }

        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] ObjectToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="Bytes"></param>         
        /// <returns></returns> 
        public static object BytesToObject(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }
    }
}
