using System.Windows;

namespace SistemaBiblioteca
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // BOTÓN LIBROS
        private void BtnAbrirLibros_Click(object sender, RoutedEventArgs e)
        {
            Libros ventana = new Libros();
            ventana.Show();
        }

        // BOTÓN PRÉSTAMOS
        private void BtnAbrirPrestamos_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Módulo de préstamos en desarrollo");
        }
    }
}