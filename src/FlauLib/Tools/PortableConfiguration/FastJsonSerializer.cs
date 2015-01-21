using fastJSON;

namespace FlauLib.Tools.PortableConfiguration
{
    /// <summary>
    /// FastJson converter for PortableConfiguration
    /// Last updated: 21.01.2015
    /// </summary>
    public class FastJsonSerializer : PortableConfiguration.ISerializer
    {
        public T Deserialize<T>(string jsonString)
        {
            return JSON.ToObject<T>(jsonString);
        }

        public string Serialize(object settings)
        {
            var parameters = new JSONParameters
            {
                UsingGlobalTypes = false,
                UseExtensions = false
            };
            return JSON.Beautify(JSON.ToJSON(settings, parameters));
        }
    }
}
