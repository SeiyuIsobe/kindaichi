using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirWatson.VisualRecog
{
    public class Image
    {
        public Classifier[] classifiers { get; set; }
        public string image { get; set; }
    }
}
