using System.Text;
using Framework.Events;
using Framework.Snapshotting;
using Newtonsoft.Json;

namespace Framework.Utils
{
    public class Serializer
    {
        public static byte[] Serialize(object value)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value, _jsonSerializerSettings));
        }

        public static IEvent TransformEvent(byte[] data)
        {
            var o = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), _jsonSerializerSettings);
            var evt = o as IEvent;
            return evt;
        }

        public static Snapshot TransformSnapshot(byte[] data)
        {
            var o = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), _jsonSerializerSettings);
            var snap = o as Snapshot;
            return snap;
        }

        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}