using System.Reflection;
using System.Xml.Linq;

namespace BasicEIP_Core.Xml
{
    public class XmlCommentReader
    {
        private readonly XDocument _xmlDoc;

        public XmlCommentReader(string xmlFilePath)
        {
            if (File.Exists(xmlFilePath))
            {
                _xmlDoc = XDocument.Load(xmlFilePath);
            }
            else
            {
                throw new FileNotFoundException($"XML documentation file not found at {xmlFilePath}");
            }
        }

        public string? GetMethodSummary(MethodInfo method)
        {
            var memberName = $"M:{method.DeclaringType?.FullName}.{method.Name}";
            var memberNode = _xmlDoc.Descendants("member")
                                    .FirstOrDefault(m => m.Attribute("name")?.Value.StartsWith(memberName) == true);

            return memberNode?.Element("summary")?.Value.Trim();
        }

        public string? GetPropertySummary(PropertyInfo info)
        {
            var memberName = $"P:{info.DeclaringType?.FullName}.{info.Name}";
            var memberNodeLists = _xmlDoc.Descendants("member");
            var memberNode = memberNodeLists.FirstOrDefault(m => m.Attribute("name")?.Value.StartsWith(memberName) == true);

            return memberNode?.Element("summary")?.Value.Trim();
        }
    }
}
