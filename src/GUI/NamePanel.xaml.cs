using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
partial class NamePanel
{
	private FixButton CheckButton=new FixButton();
	private void Adapt(StackPanel OutputPanel)
	{
		Width=OutputPanel.Width-15;
		ButtonName.Width=Width-CheckButton.Width;
	}
	public readonly String FullPath=null;
	public Boolean IsChecked
	{
		set
		{
			CheckButton.IsChecked=value;
		}
		get
		{
			return (Boolean)CheckButton.IsChecked;
		}
	}
	public NamePanel(String Name, IOType Type, StackPanel OutputPanel, Double ImageHeight=0, Double ImageWidth=0)
	{
		//Инициализируем XAML и CheckButton
		InitializeComponent();
		ViewImage.Height=ImageHeight;
		ViewImage.Width=ImageWidth;
		CheckButton.HorizontalAlignment=HorizontalAlignment.Right;
		Children.Add(CheckButton);
		Adapt(OutputPanel);
		//Устанавливаем нужную картинку
		try
		{
			switch(Type)
			{
				case IOType.File:
					//Пытаемся установить картинку по расширению
					ViewImage.Source=new BitmapImage(new Uri(Environment.CurrentDirectory+@"\NameIcons\"+Name.Substring(Name.LastIndexOf('.')+1)+".png"));
					break;
				case IOType.Directory:
					ViewImage.Source=new BitmapImage(new Uri(Environment.CurrentDirectory+@"\DirectoryIcons\Directory.png"));
					break;
				case IOType.Drive:
					ViewImage.Source=new BitmapImage(new Uri(Environment.CurrentDirectory+@"\DirectoryIcons\Drive.png"));
					break;
			}
			
		}
		catch
		{
			//Если картинки соответствующей расширениию не нашлось, или не нашлось еще какой либо картинки, ставим картинку по умолчанию
			try
			{
				ViewImage.Source=new BitmapImage(new Uri(Environment.CurrentDirectory+@"\FileIcons\File.png"));
			}
			catch
			{
			}
		}
		//Работаем с данными
		FullPath=Name;
		ViewName.Text=FileManager.GetName(Name);
		OutputPanel.SizeChanged+=delegate
		{
			Adapt(OutputPanel);
		};
	}
}