using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.Helpers
{
    public static class Json
    {
        public static async Task<T> ToObjectAsync<T>(string value, params JsonConverter[] converters)
        {
            return await Task.Run<T>(() =>
            {
                var setting = new JsonSerializerSettings();
                setting.TypeNameHandling = TypeNameHandling.All;
                setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                return JsonConvert.DeserializeObject<T>(value, setting);
            });
        }

        public static void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            string currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }

        public static async Task<string> StringifyAsync(object value, params JsonConverter[] converters)
        {
            return await Task.Run<string>(() =>
            {
                var setting = new JsonSerializerSettings();
                setting.TypeNameHandling = TypeNameHandling.Auto;
                setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                return JsonConvert.SerializeObject(value, Formatting.Indented, setting);
            });
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            try
            {
                return typeof(T).IsAssignableFrom(objectType);
            }
            catch
            {
                return false;
            }
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            try
            {
                var jObject = JObject.Load(reader);

                // Create target object based on JObject
                T target = Create(objectType, jObject);

                // Populate the object properties
                serializer.Populate(jObject.CreateReader(), target);

                return target;
            }
            catch (Exception ex)
            {
                Console.WriteLine("JsonConverter Exception");
                Console.WriteLine("Type : {0}", objectType.Name);
                Console.WriteLine("Message : {0}", ex.Message);
            }

            return null;
        }
    }
}
