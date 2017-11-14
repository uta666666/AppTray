using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppTray.Models {
    public class HotKey {

        public Key Key { get; set; }
        public ModifierKeys Modifiers { get; set; }
        

        static HotKey() {
            _filePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "hotkeysetting.json");
        }

        private static string _filePath;

        public static HotKey Create() {
            if (File.Exists(_filePath)) {
                var setting = JsonConvert.DeserializeObject<HotKey>(File.ReadAllText(_filePath));
                return new HotKey() {
                    Modifiers = setting.Modifiers,
                    Key = setting.Key
                };
            } else {
                return new HotKey() {
                    Modifiers = ModifierKeys.None,
                    Key = Key.None
                };
            }
        }

        public static void Save(HotKey hotKey) {
            var jsonStr = JsonConvert.SerializeObject(hotKey);
            File.WriteAllText(_filePath, jsonStr, Encoding.UTF8);
        }
    }
}
