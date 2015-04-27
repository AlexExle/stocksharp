﻿namespace SampleCandles
{
	using System.Windows;
	using System.Windows.Threading;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	partial class App
	{
		private void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(MainWindow, e.Exception.ToString());
			e.Handled = true;
		}
	}
}