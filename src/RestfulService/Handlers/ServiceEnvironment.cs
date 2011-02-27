using System.Configuration;

namespace RestfulService.Handlers
{
	public static class ServiceEnvironment
	{
		public static string BaseUrl { get { return ConfigurationManager.AppSettings["Application.BaseUrl"]; } }
		public static string ArtistUriStringFormat { get { return "{0}artist/{1}"; } }

		private const string FILE_PATH_FORMAT = "{0}/{1}.xml";

		public static string CreateArtistUriString(int artistId)
		{
			return string.Format(ArtistUriStringFormat, BaseUrl, artistId);
		}

		public static string GetFilePath(int id, string mainDirectory)
		{
			return string.Format(FILE_PATH_FORMAT, mainDirectory, id);
		}
	}
}