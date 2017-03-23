using System;
using System.Collections;
using System.Collections.Specialized;
static class Converter
{
	//Конвертирует коллекцию строк в массив
	public static String[] GetArray(StringCollection Collection)
	{
		String[] Array=new String[Collection.Count];
		for(Int32 i=0; i<Array.Length; ++i)
		{
			Array[i]=Collection[i];
		}
		return Array;
	}
}
