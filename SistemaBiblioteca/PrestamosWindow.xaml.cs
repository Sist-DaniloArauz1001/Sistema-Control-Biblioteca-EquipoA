using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;

namespace SistemaBiblioteca
{
	public partial class PrestamosWindow : Window
	{
		OleDbConnection conexion =
			new OleDbConnection(
				@"Provider=Microsoft.ACE.OLEDB.12.0;
                Data Source=Biblioteca.accdb");

		int idSeleccionado = 0;

		public PrestamosWindow()
		{
			InitializeComponent();

			MostrarPrestamos();
		}

		private void MostrarPrestamos()
		{
			try
			{
				conexion.Open();

				OleDbDataAdapter da =
					new OleDbDataAdapter(
						"SELECT * FROM Prestamos",
						conexion);

				DataTable dt = new DataTable();

				da.Fill(dt);

				TablaPrestamos.ItemsSource = dt.DefaultView;

				conexion.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void BtnGuardar_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				conexion.Open();

				OleDbCommand cmd =
					new OleDbCommand(
						"INSERT INTO Prestamos " +
						"(Estudiante,Libro,FechaPrestamo,FechaEntrega) " +
						"VALUES (@Estudiante,@Libro,@FechaPrestamo,@FechaEntrega)",
						conexion);

				cmd.Parameters.AddWithValue("@Estudiante", TxtEstudiante.Text);

				cmd.Parameters.AddWithValue("@Libro", TxtLibro.Text);

				cmd.Parameters.AddWithValue("@FechaPrestamo", DpPrestamo.SelectedDate);

				cmd.Parameters.AddWithValue("@FechaEntrega", DpEntrega.SelectedDate);

				cmd.ExecuteNonQuery();

				conexion.Close();

				MessageBox.Show("Préstamo guardado");

				MostrarPrestamos();

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

				OleDbCommand cmd =
					new OleDbCommand(
						"DELETE FROM Prestamos WHERE Id=@Id",
						conexion);

				cmd.Parameters.AddWithValue("@Id", idSeleccionado);

				cmd.ExecuteNonQuery();

				conexion.Close();

				MessageBox.Show("Préstamo eliminado");

				MostrarPrestamos();

				Limpiar();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void BtnActualizar_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				conexion.Open();

				OleDbCommand cmd =
					new OleDbCommand(
						"UPDATE Prestamos SET " +
						"Estudiante=@Estudiante," +
						"Libro=@Libro," +
						"FechaPrestamo=@FechaPrestamo," +
						"FechaEntrega=@FechaEntrega " +
						"WHERE Id=@Id",
						conexion);

				cmd.Parameters.AddWithValue("@Estudiante", TxtEstudiante.Text);

				cmd.Parameters.AddWithValue("@Libro", TxtLibro.Text);

				cmd.Parameters.AddWithValue("@FechaPrestamo", DpPrestamo.SelectedDate);

				cmd.Parameters.AddWithValue("@FechaEntrega", DpEntrega.SelectedDate);

				cmd.Parameters.AddWithValue("@Id", idSeleccionado);

				cmd.ExecuteNonQuery();

				conexion.Close();

				MessageBox.Show("Préstamo actualizado");

				MostrarPrestamos();

				Limpiar();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void TablaPrestamos_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (TablaPrestamos.SelectedItem != null)
			{
				DataRowView row =
					(DataRowView)TablaPrestamos.SelectedItem;

				idSeleccionado =
					Convert.ToInt32(row["Id"]);

				TxtEstudiante.Text =
					row["Estudiante"].ToString();

				TxtLibro.Text =
					row["Libro"].ToString();

				DpPrestamo.SelectedDate =
					Convert.ToDateTime(row["FechaPrestamo"]);

				DpEntrega.SelectedDate =
					Convert.ToDateTime(row["FechaEntrega"]);
			}
		}

		private void Limpiar()
		{
			TxtEstudiante.Text = "";

			TxtLibro.Text = "";

			DpPrestamo.SelectedDate = null;

			DpEntrega.SelectedDate = null;
		}
	}
}