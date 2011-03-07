namespace RestfulService.Utility.IO.Readers
{
	public interface IReader<out T>
	{
		T ReadFromFile(int id);
		bool Exists(string filePath);
	}
}