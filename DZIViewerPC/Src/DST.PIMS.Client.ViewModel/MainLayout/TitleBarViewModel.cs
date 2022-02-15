using DST.Database;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Client.ViewModel
{
    public class TitleBarViewModel : CustomBaseViewModel
    {
        public ExtendAppContext ExCurrent => ExtendAppContext.Current;
    }
}
