using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
//Класс отвечающий за графическое представление файловой системы
class Creator
{
	//StackPanel для вывода графического представления файлов и директорий
	private StackPanel OutputPanel=null;
	//TextBox для папнели переходов
	private TextBox GotoPanel=null;
	//История
	private History CurrentPath=new History();
	//Коллекция выделенных путей для копирования и перемещения
	private StringCollection PathsCollection=null;
	//Вырезать или копировать
	private Boolean IsCut=false;
	//Размеры картинки NamePanel
	private Double ImageHeight=0;
	private Double ImageWidth=0;
	//Сменить текущую директорию
	private void ReplaceDirectory(String NewPath)
	{
		CurrentPath.This=NewPath;
		if(GotoPanel!=null)
		{
			if(CurrentPath.This==null)
			{
				GotoPanel.Text=CurrentPath.This;
			}
			else
			{
				GotoPanel.Text=FileManager.GetTruePath(CurrentPath.This);
			}
		}
	}
	private void Find(String StartDirectory, String Name)
	{
		try
		{
			foreach(String AnyFile in Directory.GetFiles(StartDirectory))
			{
				if(AnyFile.Contains(Name))
				{
					OutputPanel.Children.Add(GetPanel(AnyFile, IOType.File));
				}
			}
			foreach(String AnyDirectory in Directory.GetDirectories(StartDirectory))
			{
				if(AnyDirectory.Contains(Name))
				{
					OutputPanel.Children.Add(GetPanel(AnyDirectory, IOType.Directory));
				}
				Find(AnyDirectory, Name);
			}
		}
		catch(Exception Error)
		{
			new PropertyWindow(Error.Message, "Ошибка");
		}
	}
	//Возвращает папнель с представлением элемента файловой системы
	private NamePanel GetPanel(String ButtonPath, IOType Type)
	{
		NamePanel ResultPanel=new NamePanel(ButtonPath, Type, OutputPanel, ImageHeight, ImageWidth);
		ResultPanel.ButtonName.Click+=delegate
		{
			ShowPath(ButtonPath);
		};
		return ResultPanel;
	}
	//Возвращает полные пути выделенных имен
	public StringCollection GetFixedPaths()
	{
		StringCollection FixedPathes=new StringCollection();
		foreach(NamePanel AnyButton in OutputPanel.Children)
		{
			if(AnyButton.IsChecked)
			{
				FixedPathes.Add(AnyButton.FullPath);
			}
		}
		return FixedPathes;
	}
	//Выделяет и снимает выделение, фиксирует существование выделенных имен
	public Boolean IsFixedNames
	{
		set
		{
			foreach(NamePanel AnyButton in OutputPanel.Children)
			{
				AnyButton.IsChecked=value;
			}
		}
		get
		{
			foreach(NamePanel AnyButton in OutputPanel.Children)
			{
				if(AnyButton.IsChecked)
				{
					return true;
				}
			}
			return false;
		}
	}
	//Метод открывает файлы и директории
	public void ShowPath(String Road)
	{
		try
		{
			//Если файл существует, запускаем его
			if(File.Exists(Road))
			{
				Process.Start(Road);
			}
			//Если директория существует, открываем ее
			if(Directory.Exists(Road))
			{
				//Ощищаем область представления
				OutputPanel.Children.Clear();
				foreach(String AnyDirectory in Directory.GetDirectories(Road))
				{
					OutputPanel.Children.Add(GetPanel(AnyDirectory, IOType.Directory));
				}
				foreach(String AnyFile in Directory.GetFiles(Road))
				{
					OutputPanel.Children.Add(GetPanel(AnyFile, IOType.File));
				}
				ReplaceDirectory(Road);
			}
		}
		catch(Exception Error)
		{
			//Заполняем область представления на случай если она была очищена и не заполнена вновь из за исключения
			ShowPath(CurrentPath.This);
			new PropertyWindow(FileManager.GetTruePath(Error.Message), "Ошибка");
		}
	}
	//Свойства
	public void ShowProperty()
	{
		if(IsFixedNames)
		{
			StringCollection Collection=GetFixedPaths();
			IsFixedNames=false;
			//Если был выделен один файл
			if(Collection.Count<2)
			{
				new PropertyWindow("Путь: "+FileManager.GetTruePath(Collection[0])+"\nРазмер: "+FileManager.GetSize(Collection[0])+" байт", FileManager.GetName(Collection[0]));
			}
			//Остальные случаи, включая 0
			else
			{
				new PropertyWindow("Путей: "+Collection.Count+"\nОбщий размер: "+FileManager.GetSize(Converter.GetArray(Collection))+" байт", "Свойства");
			}
		}
	}
	//Открыть с помощью
	public void ShowWith()
	{
		if(IsFixedNames&&CurrentPath.This!=CurrentPath.Root)
		{
			MiniWindow Dialog=new MiniWindow("Открыть с помощью");
			if(Dialog.ResultPath!=null&&Dialog.ResultPath.Length>0)
			{
				try
				{
					Process.Start(Dialog.ResultPath, GetFixedPaths()[0]);
				}
				catch(Exception Error)
				{
					new PropertyWindow(Error.Message, "Ошибка");
				}
			}
			IsFixedNames=false;
		}
	}
	//Домой
	public void ShowHome()
	{
		OutputPanel.Children.Clear();
		foreach(String Drive in Environment.GetLogicalDrives())
		{
			OutputPanel.Children.Add(GetPanel(Drive, IOType.Drive));
		}
		ReplaceDirectory(CurrentPath.Root);
	}
	//Назад
	public void ShowBack()
	{
		if(CurrentPath.Back==CurrentPath.Root)
		{
			ShowHome();
		}
		else
		{
			ShowPath(CurrentPath.Back);
		}
	}
	public void Find()
	{
		DialogWindow Dialog=new DialogWindow("Найти", null, false);
		if(Dialog.Text!=null&&Dialog.Text.Length>0)
		{
			OutputPanel.Children.Clear();
			GotoPanel.Text="Результаты поиска: "+'"'+Dialog.Text+'"';
			Find(CurrentPath.This, Dialog.Text);
		}
	}
	//Переименовать
	public void Rename()
	{
		StringCollection FixedPath=GetFixedPaths();
		IsFixedNames=false;
		if(FixedPath.Count>0&&CurrentPath.This!=CurrentPath.Root)
		{
			DialogWindow Dialog=new DialogWindow("Переименовать");
			if(Dialog.Text!=null&&Dialog.Text.Length>0)
			{
				try
				{
					if(Directory.Exists(FixedPath[0]))
					{
						Directory.Move(FixedPath[0], Path.GetDirectoryName(FixedPath[0])+'\\'+Dialog.Text);
					}
					if(File.Exists(FixedPath[0]))
					{
						File.Move(FixedPath[0], Path.GetDirectoryName(FixedPath[0])+'\\'+Dialog.Text);
					}
				}
				catch(Exception Error)
				{
					new PropertyWindow(Error.Message, "Ошибка");
				}
			}
		}
		ShowPath(CurrentPath.This);
	}
	//Создать
	public void Create()
	{
		if(CurrentPath.This!=CurrentPath.Root)
		{
			DialogWindow Dialog=new DialogWindow("Создать", "Это папка", true);
			if(Dialog.Text!=null&&Dialog.Text.Length>0)
			{
				try
				{
					if(Dialog.IsFix)
					{
						Directory.CreateDirectory(CurrentPath.This+'\\'+Dialog.Text);
					}
					else
					{
						File.Create(CurrentPath.This+'\\'+Dialog.Text);
					}
				}
				catch(Exception Error)
				{
					new PropertyWindow(Error.Message, "Ошибка");
				}
			}
		}
		ShowPath(CurrentPath.This);
	}
	//Удаяет выделенные пути
	public void Delete()
	{
		if(CurrentPath.This!=CurrentPath.Root)
		{
			FileManager.DeletePaths(Converter.GetArray(GetFixedPaths()));
		}
		ShowPath(CurrentPath.This);
	}
	//Копировать
	public void Copy()
	{
		if(IsFixedNames&&CurrentPath.This!=CurrentPath.Root)
		{
			PathsCollection=GetFixedPaths();
			IsCut=false;
			IsFixedNames=false;
		}
	}
	//Вырезать
	public void Cut()
	{
		if(IsFixedNames&&CurrentPath.This!=CurrentPath.Root)
		{
			PathsCollection=GetFixedPaths();
			IsCut=true;
			IsFixedNames=false;
		}
	}
	public void Paste()
	{
		if(CurrentPath.This!=CurrentPath.Root)
		{
			FileManager.PastePaths(CurrentPath.This, Converter.GetArray(PathsCollection), IsCut);
		}
		//Обновляем область представления
		ShowPath(CurrentPath.This);
	}
	public Creator(StackPanel OutputPanel, TextBox GotoPanel, Double ImageHeight=0, Double ImageWidth=0)
	{
		this.OutputPanel=OutputPanel;
		this.GotoPanel=GotoPanel;
		this.ImageHeight=ImageHeight;
		this.ImageWidth=ImageWidth;
	}
}

