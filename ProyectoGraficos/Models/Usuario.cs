using MySql.Data.MySqlClient;
using PanelControlSolar.Datos;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PanelControlSolar.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public static string Nombre { get; set; }
        public string Contraseña { get; set; }

        public bool ComprobarUsuario(string nombre, string contraseña)
        {
            bool acceso = false;

            Conexion conn = new Conexion();

            string ContraseñaCifrada = CalcularHashCifrado(contraseña);

            using (MySqlConnection conexion = conn.mySqlConnection)
            {
                string query = "SELECT id, usuario, pass " +
                                "FROM usuarios " +
                                "WHERE usuario = '" + nombre + "' AND pass = '" + ContraseñaCifrada + "'";

                MySqlCommand cmd = new MySqlCommand(query, conexion);

                conexion.Open();

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Id = Convert.ToInt32(dr["id"].ToString());
                        Nombre = dr["usuario"].ToString();
                        Contraseña = dr["pass"].ToString();
                    }

                    // Comparar las contraseñas cifradas
                    if (ContraseñaCifrada == Contraseña)
                    {
                        // Las contraseñas coinciden, el usuario tiene acceso válido
                        acceso = true;
                    }

                }
            }

            return acceso;
        }

        public bool RegistrarUsuario(string nombre, string contraseña)
        {
            bool registrado = false;

            Conexion conn = new Conexion();

            string query = "INSERT INTO usuarios (usuario, pass) VALUES (@nombre, @contraseña)";

            using (MySqlConnection conexion = conn.mySqlConnection)
            using (MySqlCommand cmd = new MySqlCommand(query, conexion))
            {

                contraseña = CalcularHashCifrado(contraseña);

                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@contraseña", contraseña);

                try
                {
                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        registrado = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al registrar usuario: " + ex.Message);
                }
            }

            return registrado;
        }

        public string CalcularHashCifrado(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public bool EliminarUsuario(string username)
        {
            bool eliminado = false;

            Conexion conn = new Conexion();

            string query = "DELETE FROM usuarios WHERE usuario = @nombre";

            using (MySqlConnection conexion = conn.mySqlConnection)
            using (MySqlCommand cmd = new MySqlCommand(query, conexion))
            {
                cmd.Parameters.AddWithValue("@nombre", username);

                try
                {
                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        eliminado = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al registrar usuario: " + ex.Message);
                }
            }

            return eliminado;
        }
    }
}