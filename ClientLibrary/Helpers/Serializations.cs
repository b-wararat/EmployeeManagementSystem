using System.Text.Json;

namespace ClientLibrary.Helpers
{
    public class Serializations
    {
        public static string SerializaObj<T>(T obj) => JsonSerializer.Serialize(obj);
        public static T DeseruzlizeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString);
        public static IList<T> DeseriazeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString);
    }
}
