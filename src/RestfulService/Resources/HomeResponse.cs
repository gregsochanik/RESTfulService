namespace RestfulService.Resources
{
	public class HomeResponse {
		public string Title { get; set; }
	}

	//[Serializable]
	//public class Album
	//{
	//    [XmlAttribute("id")]
	//    public int Id { get; set; }
	//    public Artist AlbumArtist { get; set; }
	//    public string Name { get; set; }
	//    public DateTime ReleaseDate { get; set; }
	//    public int TrackCount { get; set; }
	//}

	//[Serializable]
	//public class Track
	//{
	//    [XmlAttribute("id")]
	//    public int Id { get; set; }
	//    public Artist TrackArtist { get; set; }
	//    public Album TrackAlbum { get; set; }
	//    public string Name { get; set; }
	//    public DateTime ReleaseDate { get; set; }
	//    public int Length { get; set;}
	//}
}