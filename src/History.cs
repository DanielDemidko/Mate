using System;
using System.Collections.Specialized;
//Класс для сохранения истории переходов и осуществления навигации по ним
class History
{
	private StringCollection Paths=new StringCollection();
	private Int32 CurrentIndex=0;
	public readonly String Root=null;
	public readonly String Find="Find";
	private void Reset()
	{
		Paths.Clear();
		Paths.Add(Root);
		CurrentIndex=0;
	}
	public String Back
	{
		get
		{
			if(CurrentIndex>0)
			{
				return Paths[--CurrentIndex];
			}
			Reset();
			return Paths[CurrentIndex];
		}
	}
	public String This
	{
		set
		{
			if(value==Root)
			{
				Reset();
			}
			else
			{
				Paths.Add(This);
				Paths.Add(value);
				CurrentIndex=Paths.IndexOf(value);
			}
		}
		get
		{
			return Paths[CurrentIndex];
		}
	}
	public History()
	{
		Reset();
	}
}
