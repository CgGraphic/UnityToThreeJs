using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Json
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private JsonSerializerSettings settings;

        public NewtonsoftJsonSerializer()
        {
            settings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = false,
                        OverrideSpecifiedNames = true
                    }
                }
            };
        }

        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }
    }
}