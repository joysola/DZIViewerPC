using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Nico.Common;

namespace DST.Common.Barcode
{
    /// <summary>
    /// 条码枪无焦点获取条码信息
    /// </summary>
    public class BarcodeHook
    {
        /// <summary>
        /// 单例对象
        /// </summary>
        public static BarcodeHook Instance { get; } = new BarcodeHook();

        public delegate void BardCodeDeletegate(string barcode);
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        private BarcodeModel barCode = new BarcodeModel();
        private int hKeyboardHook = 0;
        private StringBuilder sbBarCode = new StringBuilder();

        //定义成静态，这样不会抛出回收异常
        private static HookProc hookproc;

        /// <summary>
        /// 接收到条码后的事件
        /// </summary>
        public event BardCodeDeletegate ReceiveBarcode;

        private BarcodeHook()
        {
        }

        /// <summary>
        /// 钩子响应
        /// </summary>
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            int i_calledNext = -10;
            if (nCode == 0)
            {
                EventMessageModel msg = (EventMessageModel)Marshal.PtrToStructure(lParam, typeof(EventMessageModel));
                if (wParam == 0x100)//WM_KEYDOWN=0x100
                {
                    barCode.VirtKey = msg.message & 0xff;//虚拟吗
                    barCode.ScanCode = msg.paramL & 0xff;//扫描码
                    StringBuilder strKeyName = new StringBuilder(225);
                    if (GetKeyNameText(barCode.ScanCode * 65536, strKeyName, 255) > 0)
                    {
                        barCode.KeyName = strKeyName.ToString().Trim(new char[] { ' ', '\0' });
                    }
                    else
                    {
                        barCode.KeyName = "";
                    }
                    byte[] kbArray = new byte[256];
                    uint uKey = 0;
                    GetKeyboardState(kbArray);

                    if (ToAscii(barCode.VirtKey, barCode.ScanCode, kbArray, ref uKey, 0))
                    {
                        barCode.Ascll = uKey;
                        barCode.Chr = Convert.ToChar(uKey);
                        barCode.OriginalChrs += " " + Convert.ToString(barCode.Chr);
                        barCode.OriginalAsciis += " " + Convert.ToString(barCode.Ascll);
                        barCode.OriginalBarCode += Convert.ToString(barCode.Chr);
                    }

                    TimeSpan ts = DateTime.Now.Subtract(barCode.Time);

                    if (ts.TotalMilliseconds > 30)
                    {
                        barCode = new BarcodeModel();
                        //时间戳，大于50 毫秒表示手动输入
                        //sbBarCode.Remove(0, sbBarCode.Length);
                        //sbBarCode.Append(barCode.Chr.ToString());
                        //barCode.OriginalChrs = " " + Convert.ToString(barCode.Chr);
                        //barCode.OriginalAsciis = " " + Convert.ToString(barCode.Ascll);
                        //barCode.OriginalBarCode = Convert.ToString(barCode.Chr);
                    }
                    else
                    {
                        if ((msg.message & 0xff) == 13 && sbBarCode.Length > 3)
                        {
                            barCode.BarCode = barCode.OriginalBarCode;
                            barCode.IsValid = true;
                            sbBarCode.Remove(0, sbBarCode.Length);
                        }

                        sbBarCode.Append(barCode.Chr.ToString());
                    }

                    try
                    {
                        if (ReceiveBarcode != null && barCode.IsValid)
                        {
                            //先进行 WINDOWS事件往下传
                            i_calledNext = CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
                            ReceiveBarcode(barCode.BarCode);//触发事件
                            barCode.BarCode = "";
                            barCode.OriginalChrs = "";
                            barCode.OriginalAsciis = "";
                            barCode.OriginalBarCode = "";
                            barCode = new BarcodeModel();
                        }
                    }
                    catch { }
                    finally
                    {
                        barCode.IsValid = false; //最后一定要 设置barCode无效
                        barCode.Time = DateTime.Now;
                    }
                }
            }
            if (i_calledNext == -10)
            {
                i_calledNext = CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
            }

            return i_calledNext;
        }

        //安装钩子
        public bool Start()
        {
            try
            {
                if (hKeyboardHook == 0)
                {
                    hookproc = new HookProc(KeyboardHookProc);

                    //GetModuleHandle 函数 替代 Marshal.GetHINSTANCE
                    //防止在 framework4.0中 注册钩子不成功
                    IntPtr modulePtr = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);

                    //WH_KEYBOARD_LL=13
                    //全局钩子 WH_KEYBOARD_LL
                    //  hKeyboardHook = SetWindowsHookEx(13, hookproc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                    hKeyboardHook = SetWindowsHookEx(13, hookproc, modulePtr, 0);
                }
            }
            catch(Exception e)
            {
                Logger.Error("条码钩子启动失败：" + e.Message);
            }

            return (hKeyboardHook != 0);
        }

        //卸载钩子
        public bool Stop()
        {
            try
            {
                if (hKeyboardHook != 0)
                {
                    return UnhookWindowsHookEx(hKeyboardHook);
                }
            }
            catch (Exception e)
            {
                Logger.Error("条码钩子停止失败：" + e.Message);
            }

            return true;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        [DllImport("user32", EntryPoint = "GetKeyNameText")]
        private static extern int GetKeyNameText(int IParam, StringBuilder lpBuffer, int nSize);

        [DllImport("user32", EntryPoint = "GetKeyboardState")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32", EntryPoint = "ToAscii")]
        private static extern bool ToAscii(int VirtualKey, int ScanCode, byte[] lpKeySate, ref uint lpChar, int uFlags);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);
    }
}
