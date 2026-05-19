using System.Windows;

namespace SistemaBiblioteca
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void BtnAbrirPrestamos_Click(object sender, RoutedEventArgs e)
		{
			PrestamosWindow ventana = new PrestamosWindow();

			ventana.Show();
		}
	}
}