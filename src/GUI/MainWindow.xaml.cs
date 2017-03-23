using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
partial class MainWindow
{
	private Creator Worker;
	//Адаптирует окно к новым размерам
	private void AdaptWindow()
	{
		//Ширина верхних кнопок перехода
		const Double ButtonsWidth=85;
		GotoPanel.Width=ActualWidth-ButtonsWidth;
		//Ширина скролла справа
		const Double ScrollWidth=25;
		OutputPanel.Width=ActualWidth-ScrollWidth;
	}
	private void FixButtonMove(object sender, MouseEventArgs e)
	{
		if(Worker.IsFixedNames)
		{
			((Button)sender).ToolTip="Снять выделение";
		}
		else
		{
			((Button)sender).ToolTip="Выделить все";
		}
	}
	//Изменить размеры
	private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
	{
		AdaptWindow();
	}
	//Развернуть/ нормализовать
	private void WindowStateChanged(object sender, EventArgs e)
	{
		AdaptWindow();
	}
	//Переход назад
	private void BackButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.ShowBack();
	}
	//Домой
	private void HomeButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.ShowHome();
	}
	//Переход
	private void GotoButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.ShowPath(GotoPanel.Text);
	}
	//Копировать файл
	private void CopyButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.Copy();
	}
	//Вырезать файл
	private void CutButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.Cut();
	}
	//Вставить файл
	private void PasteButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.Paste();
	}
	//Удалить файл
	private void DeleteButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.Delete();
	}
	private void FixButtonClick(Object Sender, RoutedEventArgs Args)
	{
		if(Worker.IsFixedNames)
		{
			Worker.IsFixedNames=false;
		}
		else
		{
			Worker.IsFixedNames=true;
		}
	}
	//Создать файл
	private void CreateButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.Create();
	}
	//Переименовать файл
	private void RenameButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.Rename();
	}
	//Открыть с помощью
	private void OpenButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.ShowWith();
	}
	//Свойства
	private void PropertyButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.ShowProperty();
	}
	//Искать
	private void FindButtonClick(Object Sender, RoutedEventArgs Args)
	{
		Worker.Find();
	}
	//О программе
	private void AboutButtonClick(Object Sender, RoutedEventArgs Args)
	{
		new PropertyWindow("Mate 1.3\nАвтор: Даниил Демидко\nE-Mail: DDemidko1@gmail.com\nВсе желающие могут принять участие в тестировании, сообщив о найденных багах.", "Mate");
	}
	private MainWindow()
	{
		InitializeComponent();
		Worker=new Creator(OutputPanel, GotoPanel, 35, 35);
		Worker.ShowHome();
	}
	[STAThread]
	private static void Main(String[] Argv)
	{
		new Application().Run(new MainWindow());
	}
}