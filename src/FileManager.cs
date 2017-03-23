using System;
using System.IO;
using System.Collections.Specialized;
//Статический класс для работы с файлами и директориями
static class FileManager
{
	//Возвращает все содержащиеся пути
	public static String[] GetPaths(String PathInfo)
	{
		if(Directory.Exists(PathInfo))
		{
			try
			{
				String[] Directories=Directory.GetDirectories(PathInfo);
				String[] Files=Directory.GetFiles(PathInfo);
				String[] Result=new String[Directories.Length+Files.Length];
				for(Int32 i=0; i<Directories.Length; ++i)
				{
					Result[i]=Directories[i];
				}
				for(Int32 i=Directories.Length, j=0; i<Result.Length; ++i, ++j)
				{
					Result[i]=Files[j];
				}
				return Result;
			}
			catch
			{
				return null;
			}
		}
		String[] AnyPath=new String[1];
		AnyPath[0]=PathInfo;
		return AnyPath;
	}
	//Возвращает размер пути в байтах
	public static UInt64 GetSize(String PathInfo)
	{
		UInt64 PathSize=0;
		try
		{
			foreach(String AnyPath in GetPaths(PathInfo))
			{
				try
				{
					if(File.Exists(AnyPath))
					{
						PathSize+=(UInt64)new FileInfo(AnyPath).Length;
					}
					if(Directory.Exists(AnyPath))
					{
						PathSize+=GetSize(AnyPath);
					}
				}
				catch
				{
				}
			}
		}
		catch
		{
		}
		return PathSize;
	}
	//Возвращает размер путей
	public static UInt64 GetSize(String[] Paths)
	{
		UInt64 AllSize=0;
		foreach(String AnyPath in Paths)
		{
			AllSize+=GetSize(AnyPath);
		}
		return AllSize;
	}
	//Возвращает имя файла или директории
	public static String GetName(String Road)
	{
		if(Road[Road.Length-1]=='/'||Road[Road.Length-1]=='\\')
		{
			return Road.Remove(Road.Length-1, 1);
		}
		return Path.GetFileName(Road);
	}
	//Метод вставляет файлы, использует рекурсию
	public static void PastePaths(String PasteDirectory, String[] Collection, Boolean IsCut)
	{
		foreach(String Road in Collection)
		{
			try
			{
				if(File.Exists(Road))
				{
					File.Copy(Road, PasteDirectory+'\\'+GetName(Road));
					if(IsCut)
					{
						File.Delete(Road);
					}
				}
				if(Directory.Exists(Road))
				{
					PastePaths(Directory.CreateDirectory(PasteDirectory+'\\'+GetName(Road)).FullName, GetPaths(Road), IsCut);
					if(IsCut)
					{
						Directory.Delete(Road);
					}
				}
			}
			catch
			{
			}
		}
	}
	//Возвращает строку где в качестве разделителя используется правый слэш
	public static String GetTruePath(String SourcePath)
	{
		Boolean FixFound=false;
		String Result=null;
		foreach(Char Symbol in SourcePath)
		{
			//Помимо замены левого слэша правым, отсеиваем так же повторы
			if((Symbol=='\\'||Symbol=='/')&&!FixFound)
			{
				Result+='/';
				FixFound=true;
				continue;
			}
			if(Symbol!='\\'&&Symbol!='/')
			{
				Result+=Symbol;
				FixFound=false;
			}
		}
		return Result;
	}
	public static void DeletePaths(String[] Paths)
	{
		foreach(String AnyPath in Paths)
		{
			try
			{
				if(File.Exists(AnyPath))
				{
					File.Delete(AnyPath);
				}
				if(Directory.Exists(AnyPath))
				{
					Directory.Delete(AnyPath, true);
				}
			}
			catch
			{
			}
		}
	}
}

