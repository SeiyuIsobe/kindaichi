using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirWatson.VisualRecog
{
    public class Result
    {
        public int custom_classes { get; set; }
        public Image[] images { get; set; }
        public int images_processed { get; set; }
    }
}
