using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirWatson.VisualRecog
{
    public class Class
    {
        [JsonProperty("class")]
        public string _class { get; set; }
        public float score { get; set; }
    }
}
