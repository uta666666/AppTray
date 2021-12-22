using AppTray.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTray.Models
{
    public class SystemSetting : BindableBase
    {
        private bool _isOpenOnTaskBar = true;
        public bool IsOpenOnTaskBar {
            get
            {
                return _isOpenOnTaskBar;
            }
            set
            {
                SetProperty(ref _isOpenOnTaskBar, value);
            }
        }

        private double _opacity = 1;
        public double Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                SetProperty(ref _opacity, value);
            }
        }

        public SystemSetting Load(string directory)
        {
            string settingFile = Path.Combine(directory, "syssettings.json");
            if (File.Exists(settingFile))
            {
                return JsonConvert.DeserializeObject<SystemSetting>(File.ReadAllText(settingFile, Encoding.UTF8));
            }
            else
            {
                return this;
            }
        }

        public void Save(string directory)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            var jsonString = JsonConvert.SerializeObject(this, setting);
            File.WriteAllText(Path.Combine(directory, "syssettings.json"), jsonString, Encoding.UTF8);
        }
    }
}
