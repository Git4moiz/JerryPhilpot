using Newtonsoft.Json;
namespace UwpClient.Helpers
{
    public class SerializationHelper
    {
        public JsonSerializerSettings Settings { get; } = new JsonSerializerSettings()
        {
            Formatting = Formatting.None,
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        public string Serialize(object value)
        {
            if (value == null || value is string s && string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(value);
        }

        public bool TrySerialize(object parameter, out string result)
        {
            try
            {
                result = Serialize(parameter);
                return true;
            }
            catch
            {
                result = default(string);
                return false;
            }
        }

        public object Deserialize(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return JsonConvert.DeserializeObject(value, Settings);
        }

        public T Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value, Settings);
        }

        public bool TryDeserialize<T>(string value, out T result)
        {
            try
            {
                result = Deserialize<T>(value);
                return (result != null);
            }
            catch
            {
                result = default(T);
                return false;
            }
        }
    }
}
