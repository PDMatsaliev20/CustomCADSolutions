using System.Text;
using System.Xml.Serialization;

namespace CustomCADSolutions.App.Extensions
{
    public class XmlExtension
    {
        public static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new(rootName);
            XmlSerializer serializer = new(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            T deserializedDto = (T)serializer.Deserialize(reader);

            return deserializedDto;
        }

        public static string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new();

            XmlRootAttribute xmlRoot = new(rootName);
            XmlSerializer serializer = new(typeof(T), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, obj, namespaces);

            return sb.ToString().Trim();
        }
    }
}
