using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silo.Config
{

    public class Rootobject
    {
        public Settings SiloConfig { get; set; }
    }

    public class Settings
    {
        public static string SettingsName = "Settings";
        public string Test { get; set; }
        public string SimpleTableStorage { get; set; }
        public string SimpleBlobStorage { get; set; }
    }

}
