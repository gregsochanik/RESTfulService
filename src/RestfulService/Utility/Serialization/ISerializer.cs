using System.Xml.XPath;

namespace RestfulService.Utility.Serialization
{
	public interface ISerializer<T>
	{
		T DeSerialize(IXPathNavigable document);
		IXPathNavigable Serialize(T serializableObject);
	}
}