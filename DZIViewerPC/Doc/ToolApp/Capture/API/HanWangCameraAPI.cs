using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class HanWangCameraAPI
    {
        [DllImport("HWCameraLib.dll", EntryPoint = "HWInit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int HWInit();

        [DllImport("HWCameraLib.dll", EntryPoint = "HWEnableAutoRotate", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWEnableAutoRotate(int idx, bool bOpen);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWOpenCamera", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWOpenCamera(int idx, IntPtr handle);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWCloseCamera", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWCloseCamera(int idx);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWCapture", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWCapture(int idx, string str);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWGetCameraCount", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWGetCameraCount();

        [DllImport("HWCameraLib.dll", EntryPoint = "HWGetCameraName", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HWGetCameraName(int idx);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWShowVideoProp", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void HWShowVideoProp(int idx);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWGetResCount", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWGetResCount(int idx);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWGetResWidth", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWGetResWidth(int idx, int iRes);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWGetResHeight", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWGetResHeight(int idx, int iRes);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWSetResolution", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWSetResolution(int idx, int iRes);

        //设置曝光度
        [DllImport("HWCameraLib.dll", EntryPoint = "HWSetExposure", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void HWSetExposure(int idx, bool bAuto, int val);

        //清空PDF文件池
        [DllImport("HWCameraLib.dll", EntryPoint = "HWClearPDFPool", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void HWClearPDFPool();

        //添加要生成PDF的图片文件, 每次只能添加一个, 可以多次添加
        [DllImport("HWCameraLib.dll", EntryPoint = "HWAddPdfImageFile", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void HWAddPdfImageFile(string filename);

        //生成PDF文件
        [DllImport("HWCameraLib.dll", EntryPoint = "HWCreatePDF", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool HWCreatePDF(string pdfname);

        //鼠标左键按下
        [DllImport("HWCameraLib.dll", EntryPoint = "HWOnLMousePress", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWOnLMousePress(IntPtr handle, int x, int y);

        //鼠标左键弹起
        [DllImport("HWCameraLib.dll", EntryPoint = "HWOnLMouseRelease", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWOnLMouseRelease(IntPtr handle, int x, int y);

        //鼠标移动
        [DllImport("HWCameraLib.dll", EntryPoint = "HWOnMouseMoving", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWOnMouseMoving(IntPtr handle, int x, int y);

        //设置鼠标的操作模式
        [DllImport("HWCameraLib.dll", EntryPoint = "HWSetMouseMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWSetMouseMode(int idx, int mode);

        //设置旋转角度
        [DllImport("HWCameraLib.dll", EntryPoint = "HWSetRotate", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWSetRotate(int idx, int index);


        [DllImport("HWCameraLib.dll", EntryPoint = "HWOnWheel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWOnWheel(IntPtr handle, int zDelta);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWZoomIn", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWZoomIn(int idx);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWZoomOut", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int HWZoomOut(int idx);

        [DllImport("HWCameraLib.dll", EntryPoint = "HWReadIDCard", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool HWReadIDCard();

        [DllImport("HWCameraLib.dll", EntryPoint = "HWGetIDName", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr HWGetIDName();
    }
}
