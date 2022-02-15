using DST.Common.Converter;
using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DST.Database.Model
{
    public class PhysDistReceiptHumanModel : ObservableObject
    { 
        public string mailNo { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? deliveryDate { get; set; }

        public int? amount { get; set; }

        [Notification]
        public ObservableCollection<Sample> sampleVOList { get; set; } = new ObservableCollection<Sample>();
    }
}
