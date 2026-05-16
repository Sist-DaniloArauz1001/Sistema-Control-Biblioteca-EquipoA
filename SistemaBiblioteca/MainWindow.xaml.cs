using System.Windows;

namespace SistemaBiblioteca
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Método para el botón de Libros (Asignado a Josué)
        private void BtnAbrirLibros_Click(object sender, RoutedEventArgs e)
        {
            // Más adelante aquí pondremos: 
            // VentanaLibros ventana = new VentanaLibros();
            // ventana.Show();

            MessageBox.Show("Módulo de Gestión de Libros.\n\n(Josué conectará su ventana aquí)", "NEXUS LIBRARY", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Método para el botón de Préstamos (Asignado a Alberto)
        private void BtnAbrirPrestamos_Click(object sender, RoutedEventArgs e)
        {
            // Más adelante aquí pondremos: 
            // VentanaPrestamos ventana = new VentanaPrestamos();
            // ventana.Show();

            MessageBox.Show("Módulo de Control de Préstamos.\n\n(Alberto conectará su ventana aquí)", "NEXUS LIBRARY", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}