using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace SistemaBiblioteca
{
    public partial class Libros : Window
    {
        OleDbConnection conexion;
        int idLibroSeleccionado = 0;

        public Libros()
        {
            InitializeComponent();

            conexion = new OleDbConnection(
                @"Provider=Microsoft.ACE.OLEDB.12.0;
                Data Source=Biblioteca.accdb");

            MostrarLibros();
        }

        private void MostrarLibros()
        {
            try
            {
                conexion.Open();

                OleDbDataAdapter da =
                    new OleDbDataAdapter(
                        "SELECT * FROM Libros",
                        conexion);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgLibros.ItemsSource = dt.DefaultView;

                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Open();

                string query =
                    "INSERT INTO Libros " +
                    "(Titulo, Autor, Categoria, Stock) " +
                    "VALUES (@Titulo, @Autor, @Categoria, @Stock)";

                OleDbCommand cmd =
                    new OleDbCommand(query, conexion);

                cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                cmd.Parameters.AddWithValue("@Autor", txtAutor.Text);
                cmd.Parameters.AddWithValue("@Categoria", txtCategoria.Text);
                cmd.Parameters.AddWithValue("@Stock", txtStock.Text);

                cmd.ExecuteNonQuery();

                conexion.Close();

                MessageBox.Show("Libro agregado");

                MostrarLibros();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Open();

                string query =
                    "UPDATE Libros SET " +
                    "Titulo=@Titulo, " +
                    "Autor=@Autor, " +
                    "Categoria=@Categoria, " +
                    "Stock=@Stock " +
                    "WHERE IdLibro=@IdLibro";

                OleDbCommand cmd =
                    new OleDbCommand(query, conexion);

                cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                cmd.Parameters.AddWithValue("@Autor", txtAutor.Text);
                cmd.Parameters.AddWithValue("@Categoria", txtCategoria.Text);
                cmd.Parameters.AddWithValue("@Stock", txtStock.Text);
                cmd.Parameters.AddWithValue("@IdLibro", idLibroSeleccionado);

                cmd.ExecuteNonQuery();

                conexion.Close();

                MessageBox.Show("Libro editado");

                MostrarLibros();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Open();

                string query =
                    "DELETE FROM Libros WHERE IdLibro=@IdLibro";

                OleDbCommand cmd =
                    new OleDbCommand(query, conexion);

                cmd.Parameters.AddWithValue("@IdLibro", idLibroSeleccionado);

                cmd.ExecuteNonQuery();

                conexion.Close();

                MessageBox.Show("Libro eliminado");

                MostrarLibros();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgLibros_SelectionChanged(object sender,
            System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (dgLibros.SelectedItem != null)
                {
                    DataRowView row =
                        (DataRowView)dgLibros.SelectedItem;

                    idLibroSeleccionado =
                        Convert.ToInt32(row["IdLibro"]);

                    txtTitulo.Text =
                        row["Titulo"].ToString();

                    txtAutor.Text =
                        row["Autor"].ToString();

                    txtCategoria.Text =
                        row["Categoria"].ToString();

                    txtStock.Text =
                        row["Stock"].ToString();
                }
            }
            catch
            {

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
        }
    }
}