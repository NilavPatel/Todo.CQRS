using System.Text;
using Newtonsoft.Json;

namespace Framework.Utils
{
    public class Serializer
    {
        public static byte[] Serialize(object value)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value, _jsonSerializerSettings));
        }

        public static T Deserialize<T>(byte[] data)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data), _jsonSerializerSettings);
        }

        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}