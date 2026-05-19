using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace SistemaBiblioteca
{

    public partial class Libros : Window
    {
        // Ruta correcta apuntando a la carpeta Data en la raíz del proyecto
        string cadenaConexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Data\Base de datos.accdb;";
        int idLibroSeleccionado = 0;

        public Libros()
        {
            InitializeComponent();
            // Ya no llamamos a MostrarLibros() aquí, se cargará solo con el botón
        }

        // NUEVO: Evento para el botón que mostrará la tabla
        private void BtnMostrar_Click(object sender, RoutedEventArgs e)
        {
            MostrarLibros();
        }

        private void txtBuscar_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Verificamos que el DataGrid tenga datos antes de intentar buscar
            if (dgLibros.ItemsSource != null)
            {
                // Obtenemos la vista actual de los datos
                DataView vistaDatos = (DataView)dgLibros.ItemsSource;

                // Filtramos buscando coincidencias en la columna Titulo
                vistaDatos.RowFilter = $"Titulo LIKE '%{txtBuscar.Text}%'";
            }
        }

        private void MostrarLibros()
        {
            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    // Usamos Id_libro tal cual viene en la base de datos de Miguel
                    string query = "SELECT Id_libro, Titulo, Autor, Categoria, Stock FROM Libros";

                    OleDbDataAdapter da = new OleDbDataAdapter(query, conexion);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgLibros.ItemsSource = dt.DefaultView;
                } // El bloque 'using' cierra automáticamente la conexión aquí
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // VALIDACIÓN: Asegurar que los campos obligatorios no estén vacíos
            if (string.IsNullOrWhiteSpace(txtTitulo.Text) || string.IsNullOrWhiteSpace(txtAutor.Text))
            {
                MessageBox.Show("Por favor, llene al menos el Título y el Autor.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO Libros (Titulo, Autor, Categoria, Stock) VALUES (@Titulo, @Autor, @Categoria, @Stock)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                        cmd.Parameters.AddWithValue("@Autor", txtAutor.Text);
                        cmd.Parameters.AddWithValue("@Categoria", txtCategoria.Text);

                        // Si dejan el stock vacío, se guarda un 0 en lugar de crashear
                        int stock = string.IsNullOrWhiteSpace(txtStock.Text) ? 0 : Convert.ToInt32(txtStock.Text);
                        cmd.Parameters.AddWithValue("@Stock", stock);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Libro agregado exitosamente.", "Operación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                MostrarLibros();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el libro: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            // VALIDACIÓN: Asegurar que hayan seleccionado un libro del DataGrid
            if (idLibroSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un libro de la tabla para editarlo.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "UPDATE Libros SET Titulo=@Titulo, Autor=@Autor, Categoria=@Categoria, Stock=@Stock WHERE Id_libro=@IdLibro";

                    using (OleDbCommand cmd = new OleDbCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                        cmd.Parameters.AddWithValue("@Autor", txtAutor.Text);
                        cmd.Parameters.AddWithValue("@Categoria", txtCategoria.Text);

                        int stock = string.IsNullOrWhiteSpace(txtStock.Text) ? 0 : Convert.ToInt32(txtStock.Text);
                        cmd.Parameters.AddWithValue("@Stock", stock);
                        cmd.Parameters.AddWithValue("@IdLibro", idLibroSeleccionado);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Los datos del libro se actualizaron correctamente.", "Operación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                MostrarLibros();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el libro: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (idLibroSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un libro de la tabla para eliminarlo.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // CONFIRMACIÓN: Mensaje para evitar borrados accidentales
            MessageBoxResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar este libro de la base de datos?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (respuesta == MessageBoxResult.Yes)
            {
                try
                {
                    using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                    {
                        conexion.Open();
                        string query = "DELETE FROM Libros WHERE Id_libro=@IdLibro";

                        using (OleDbCommand cmd = new OleDbCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@IdLibro", idLibroSeleccionado);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Libro eliminado de la base de datos.", "Operación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                    MostrarLibros();
                    Limpiar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el libro: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void dgLibros_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (dgLibros.SelectedItem != null)
                {
                    DataRowView row = (DataRowView)dgLibros.SelectedItem;

                    idLibroSeleccionado = Convert.ToInt32(row["Id_libro"]);
                    txtTitulo.Text = row["Titulo"].ToString();
                    txtAutor.Text = row["Autor"].ToString();
                    txtCategoria.Text = row["Categoria"].ToString();
                    txtStock.Text = row["Stock"].ToString();
                }
            }
            catch
            {
                // Ignorar excepciones al perder la selección
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            txtCategoria.Clear();
            txtStock.Clear();
            idLibroSeleccionado = 0;

            // Quitar la selección visual del DataGrid
            if (dgLibros != null)
            {
                dgLibros.SelectedItem = null;
            }
        }
    }
}