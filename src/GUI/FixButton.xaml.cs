using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
//Кнопка для выделения файлов или директорий
partial class FixButton
{
	public FixButton()
	{
		InitializeComponent();
		Checked+=delegate
		{
			try
			{
				Image Icon=new Image();
				Icon.Source=new BitmapImage(new Uri(Environment.CurrentDirectory+@"\ToolIcons\Original\Delete.tif"));
				Content=Icon;
			}
			catch
			{
				Content='+';
			}
		};
		Unchecked+=delegate
		{
			Content=null;
		};
	}
}