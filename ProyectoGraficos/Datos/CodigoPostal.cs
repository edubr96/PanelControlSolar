using MySql.Data.MySqlClient;
using PanelControlSolar.Datos;
using PanelControlSolar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanelControlSolar.Datos
{
    public class CodigoPostal
    {
        public static string Codigopostal { get; set; }

        public bool ModificarCodigo(string codigoPostal)
        {
            bool actualizado = false;

            Conexion conn = new Conexion();

            using (MySqlConnection conexion = conn.mySqlConnection)
            {
                string query = "UPDATE posicion " +
                               "SET codigopostal = @codigoPostal";

                MySqlCommand cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@codigoPostal", codigoPostal);

                conexion.Open();

                int filasAfectadas = cmd.ExecuteNonQuery();
                if (filasAfectadas > 0)
                {
                    actualizado = true;
                    Codigopostal = codigoPostal;
                
                } 
            }

            return actualizado;
        }

        public string ObtenerCodigoPostal()
        {
            string codigoPostal = "00000";
            bool sinCodigo = false;

            Conexion conn = new Conexion();

            using (MySqlConnection conexion = conn.mySqlConnection)
            {
                string query = "SELECT codigopostal " +
                               "FROM posicion";

                MySqlCommand cmd = new MySqlCommand(query, conexion);

                conexion.Open();

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        codigoPostal = dr["codigopostal"].ToString();
                        sinCodigo = true;
                    } 
                }

                if (!sinCodigo)
                {
                    query = "INSERT INTO posicion VALUES (@codigoPostal) ";
                    cmd = new MySqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@codigoPostal", "00000");
                    cmd.ExecuteNonQuery();
                }

                Codigopostal = codigoPostal;
            }

            return codigoPostal;
        }

    }
}