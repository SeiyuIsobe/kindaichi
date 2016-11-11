using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Shared
{
    public class RemoexecSetting
    {
        private RemoexecParameter _param = null;

        public RemoexecSetting(string id)
        {
            string line = string.Empty;

            // 最初は読込み
            try
            {
                using (StreamReader reader = new StreamReader($"setting_{id}.ini"))
                {
                    line = reader.ReadToEnd();

                    _param = JsonConvert.DeserializeObject<RemoexecParameter>(line);
                }

                if (true == string.IsNullOrEmpty(line))
                {
                    RemoexecParameter param = new RemoexecParameter();

                    string json = JsonConvert.SerializeObject(param, Formatting.Indented);

                    using (StreamWriter writer = new StreamWriter($"setting_{id}.ini", false, Encoding.ASCII))
                    {
                        writer.WriteLine(json);
                    }
                }
                else
                {

                }
            }
            catch
            {
                RemoexecParameter param = new RemoexecParameter();

                string json = JsonConvert.SerializeObject(param, Formatting.Indented);

                using (StreamWriter writer = new StreamWriter($"setting_{id}.ini", false, Encoding.ASCII))
                {
                    writer.WriteLine(json);
                }
            }
            

            
                
        }

        public string EndPoint
        {
            get
            {
                if (null == _param) return string.Empty;
                return _param.EndPoint;
            }
        }
    }

    class RemoexecParameter
    {
        public string EndPoint { get; set; } = "172.31.61.147";
    }
}
