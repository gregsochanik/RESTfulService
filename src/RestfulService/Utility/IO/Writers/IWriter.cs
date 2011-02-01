namespace RestfulService.Utility.IO.Writers
{
	public interface IWriter<in T>
	{
		void CreateFile(T item);
		void UpdateFile(T artist);
		void DeleteFile(int id);
	}
}