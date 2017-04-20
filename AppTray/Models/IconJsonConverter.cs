using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppTray.Models {
    public class IconJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return typeof(Icon).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            string iconBase64String = reader.Value.ToString();
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(iconBase64String))) {
                Bitmap bmp = (Bitmap)Bitmap.FromStream(ms);
                return Icon.FromHandle(bmp.GetHicon());
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (!CanConvert(value.GetType())) {
                return;
            }
            
            using (var ms = new MemoryStream()) {
                Bitmap bmp = (value as Icon).ToBitmap();
                bmp.Save(ms, ImageFormat.Png);
                byte[] binary = ms.GetBuffer();
                writer.WriteValue(Convert.ToBase64String(binary));
            }
        }
    }
}
