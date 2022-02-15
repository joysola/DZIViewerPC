using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class ExamedReturnModel : ObservableObject
    {
        public int? pages { get; set; }
        public int? total { get; set; }
        public int? size { get; set; }
        public int? current { get; set; }

        [Notification]
        public ObservableCollection<ExamedModel> records { get; set; }
    }
}
