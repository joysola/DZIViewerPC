using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class MapMarks : ObservableObject
    {
        public string key { get; set; }

        [Notification]
        public ObservableCollection<MarkModel> markList { get; set; } = new ObservableCollection<MarkModel>();

        [Notification]
        [JsonIgnore]
        public ObservableCollection<MarkModel> selectedMarkList { get; set; } = new ObservableCollection<MarkModel>();
    }

    public class MarkModel : ObservableObject
    {
        public string valueShort { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }
}
