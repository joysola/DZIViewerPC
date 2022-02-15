using DST.Common.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class SignExpressByHand
    {
        public Nullable<int> amount { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<DateTime> deliveryDate { get; set; }
        public string mailNo { get; set; }
        public List<Sample> sampleVOList { get; set; }
    }
}
