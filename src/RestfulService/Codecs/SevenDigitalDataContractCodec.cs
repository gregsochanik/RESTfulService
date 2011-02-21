using System.Text;
using System.Xml;
using System.Xml.Serialization;
using OpenRasta.Codecs;
using OpenRasta.Web;

namespace RestfulService.Codecs {
	[MediaType("application/vnd.7digital+xml")]
	public class SevenDigitalDataContractCodec : IMediaTypeWriter {
		public object Configuration { get;set;}

		public void WriteTo(object entity, IHttpEntity response, string[] codecParameters) {

			if (entity == null)
				return;
			
			bool isError = (response.Errors.Count > 0);
			string status = isError ? "error" : "ok";

			var writerSettings = new XmlWriterSettings { OmitXmlDeclaration = true, Encoding = Encoding.UTF8, NamespaceHandling = NamespaceHandling.OmitDuplicates };
			
			using (var xmlTextWriter = XmlWriter.Create(response.Stream, writerSettings)) {

				xmlTextWriter.WriteStartDocument();
				xmlTextWriter.WriteStartElement("response");
				xmlTextWriter.WriteAttributeString("xsi", "noNamespaceSchemaLocation", "", "http://api.7digital.com/1.2/static/7digitalAPI.xsd");
				xmlTextWriter.WriteAttributeString("status", status);
				xmlTextWriter.WriteAttributeString("version", "1.2");
				xmlTextWriter.WriteAttributeString("xmlns", "xsd", "", "http://www.w3.org/2001/XMLSchema");
				xmlTextWriter.WriteAttributeString("xmlns", "xsi", "", "http://www.w3.org/2001/XMLSchema-instance");

				OutputEntity(entity, response, isError, xmlTextWriter);
				
				xmlTextWriter.WriteEndElement();
			}
		}

		private static void OutputEntity(object entity, IHttpEntity response, bool isError, XmlWriter xmlTextWriter) {
			if (!isError) {
				var x = new XmlSerializer(entity.GetType());
				x.Serialize(xmlTextWriter, entity);
			} else {
				xmlTextWriter.WriteStartElement("error", "");
				foreach (var error in response.Errors) {
					xmlTextWriter.WriteStartElement("errorMessage");
					xmlTextWriter.WriteAttributeString("code", "1001");
					xmlTextWriter.WriteString(error.Message);
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
			}
		}
	}
}