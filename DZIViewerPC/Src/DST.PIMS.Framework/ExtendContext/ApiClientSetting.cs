using DST.Controls;
using DST.PIMS.Framework.ExtendContext;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Model;
using Nico.Common;
using System.Threading.Tasks;
using System.Windows;

namespace DST.PIMS.Framework
{
    public class ApiClientSetting
    {
        /// <summary>
        /// 配置HttpClientEx的总方法
        /// </summary>
        public static void SetHttpClientEx()
        {
            // 设置最小线程池可用线程数，防止初始化加载大量字典时线程不够，导致速度下降
            System.Threading.ThreadPool.SetMinThreads(20, 20);
            // 初始化
            HttpClientEx.InitApiClient(CommonConfiguration.BaseApi, HttpHandlerEnum.WinHttpHandler);
            HttpClientEx.SetTimeout(60 * 1000);
            // 设置版本请求头
            HttpClientEx.SetCustomRequestHead("n-d-version", CommonConfiguration.ApiVersion);
            // 测速(简易模式)
            HttpClientEx.SetBenchmark(desc =>
            {
                Logger.Info($"接口测速：{desc}");
            });
            // 预处理json
            HttpClientEx.SetPrePorcess(typeof(ApiResponse<object>), data =>
            {
                if (!data.Success)
                {
                    var message = $"WebApi访问失败！原因：{data.Msg}";
                    Task.Run(() => Logger.Info(message));
                    if (data.Code != "200")
                    {
                        // 针对Api访问的请求处理
                        Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            ConfirmMessageBox.Show("", message, MessageBoxButton.OK, MessageBoxImage.Error, true, 2000);
                        });
                    }
                }
            });
        }
    }
}
