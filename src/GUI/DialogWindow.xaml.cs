using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
public partial class DialogWindow
{
	private FixButton CheckButton;
	private void OkButtonClick(object sender, RoutedEventArgs e)
	{
		Text=TextResult.Text;
		Close();
	}
	public Boolean IsFix
	{
		get
		{
			return (Boolean)CheckButton.IsChecked;
		}
	}
	public String Text
	{
		private set;
		get;
	}
	public DialogWindow(String TextTitle, String TextForCheck=null, Boolean IsCheckButton=false)
	{
		InitializeComponent();
		if(IsCheckButton)
		{
			TextBlock CheckText=new TextBlock();
			CheckButton=new FixButton();
			CheckText.Background=Brushes.Transparent;
			CheckText.FontSize=15;
			CheckText.Text=TextForCheck;
			CheckButton.Height=25;
			CheckButton.Margin=new Thickness(5, 0, 0, 0);
			WrapPanel UnionCheck=new WrapPanel();
			UnionCheck.Children.Add(CheckText);
			UnionCheck.Children.Add(CheckButton);
			UnionCheck.Margin=new Thickness(0, 10, 0, 0);
			UnionCheck.VerticalAlignment=VerticalAlignment.Top;
			MainPanel.Children.Add(UnionCheck);
		}
		Title=TextTitle;
		ShowDialog();
	}
}