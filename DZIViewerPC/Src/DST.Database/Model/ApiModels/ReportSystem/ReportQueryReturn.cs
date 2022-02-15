using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class ReportQueryReturn
    {
        public ObservableCollection<Report> records { get; set; } = new ObservableCollection<Report>();
        public int? pages { get; set; }
        public int? total { get; set; }
        public int? size { get; set; }
        public int? current { get; set; }
    }
}
