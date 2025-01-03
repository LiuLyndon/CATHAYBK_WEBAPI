using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.XPath;

namespace BasicEIP_Core.Swagger
{
    public class AddModelSummarySchemaFilter : ISchemaFilter
    {
        private readonly XPathDocument _xmlComments;
        private readonly XPathNavigator _xmlNavigator;

        public AddModelSummarySchemaFilter(string xmlPath)
        {
            _xmlComments = new XPathDocument(xmlPath);
            _xmlNavigator = _xmlComments.CreateNavigator();
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context.Type == null)
                return;

            // 找到類別的 XML 註解
            var typeNode = _xmlNavigator.SelectSingleNode(
                $"/doc/members/member[@name='T:{context.Type.FullName}']");

            if (typeNode != null)
            {
                var summaryNode = typeNode.SelectSingleNode("summary");
                if (summaryNode != null)
                {
                    schema.Description = summaryNode.InnerXml.Trim();
                }
            }

            // 為每個屬性添加描述
            foreach (var property in schema.Properties)
            {
                var propertyNode = _xmlNavigator.SelectSingleNode(
                    $"/doc/members/member[@name='P:{context.Type.FullName}.{property.Key}']");

                if (propertyNode != null)
                {
                    var propertySummary = propertyNode.SelectSingleNode("summary");
                    if (propertySummary != null)
                    {
                        property.Value.Description = propertySummary.InnerXml.Trim();
                    }
                }
            }
        }
    }
}
