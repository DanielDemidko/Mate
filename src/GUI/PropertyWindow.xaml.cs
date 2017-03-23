using System;
public partial class PropertyWindow
{
	public PropertyWindow(String PropertyText, String TextTitle)
	{
		InitializeComponent();
		Title=TextTitle;
		TextProperty.Text=PropertyText;
		ShowDialog();
	}
}