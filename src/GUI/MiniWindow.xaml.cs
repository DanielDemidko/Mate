using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
public partial class MiniWindow
{
	private Creator Worker=null;
	//Переход назад
	private void BackButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.ShowBack();
	}
	private void OkButtonClick(object sender, RoutedEventArgs e)
	{
		if(Worker.IsFixedNames)
		{
			ResultPath=Worker.GetFixedPaths()[0];
		}
		Close();
	}
	public String ResultPath
	{
		private set;
		get;
	}
	public MiniWindow(String TextTitle)
	{
		InitializeComponent();
		Worker=new Creator(OutputPanel, null, 20, 20);
		Title=TextTitle;
		Worker.ShowHome();
		ShowDialog();
	}
}