using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AppTray.Commons {
    class BitmapSourceJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return typeof(BitmapSource).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (reader.Value == null) {
                return null;
            }
            //return null;
            string iconBase64String = reader.Value.ToString();
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(iconBase64String))) {
                var decorder = new PngBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);
                return decorder.Frames[0];
            }

            //var result = new BitmapImage();
            //using (var stream = new MemoryStream(Convert.FromBase64String(iconBase64String))) {
            //    result.BeginInit();
            //    result.CacheOption = BitmapCacheOption.OnLoad;
            //    result.CreateOptions = BitmapCreateOptions.None;
            //    result.StreamSource = stream;
            //    result.EndInit();
            //    result.Freeze();    // 非UIスレッドから作成する場合、Freezeしないとメモリリークするため注意
            //}
            //return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (!CanConvert(value.GetType())) {
                return;
            }
            using (var ms = new MemoryStream()) {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((value as BitmapSource)));
                encoder.Save(ms);
                writer.WriteValue(Convert.ToBase64String(ms.ToArray()));
            }
        }
    }
}
