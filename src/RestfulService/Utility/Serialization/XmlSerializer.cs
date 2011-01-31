using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace RestfulService.Utility.Serialization
{
	public class XmlSerializer<T> : ISerializer<T>
	{
		public T DeSerialize(IXPathNavigable document) {
			if (document == null)
				throw new ArgumentNullException("document");

			var typedDocument = document as XmlDocument;

			if (typedDocument == null || typedDocument.DocumentElement == null)
				throw new ArgumentNullException("document");

			var reader = new XmlNodeReader(typedDocument.DocumentElement);
			var ser = new XmlSerializer(typeof(T));
			object obj = ser.Deserialize(reader);
			// Then you just need to cast obj into whatever type it is eg:
			var instance = (T)obj;

			return instance;
		}

		public IXPathNavigable Serialize(T serializableObject) {
			if (serializableObject == null)
				throw new ArgumentNullException("serializableObject");

			Type obj = serializableObject.GetType();

			if (!obj.IsSerializable)
				throw new ArgumentException(String.Format("The object passed is not serializable: {0}", obj.Name));

			var doc = new XmlDocument();

			using (var s = new MemoryStream()) {
				var x = new XmlSerializer(serializableObject.GetType());
				x.Serialize(s, serializableObject);
				s.Position = 0;
				doc.Load(s);
				s.Close();
			}

			return doc;
		}
	}
}