using System;
using System.Collections.Generic;
using PanelControlSolar.Models;
using MySql.Data.MySqlClient;

namespace PanelControlSolar.Datos
{
    public class DT_DatosTiempo
    {
        public List<ListaDatosTiempo> RetornarDatosTiempo(DateTime fechaMinima, DateTime fechaMaxima)
        {
            List<ListaDatosTiempo> objLista = new List<ListaDatosTiempo>();

            Conexion conn = new Conexion();

            using (MySqlConnection conexion = conn.mySqlConnection)         
            {
                var fMinima = fechaMinima.ToString("yyy/MM/dd HH:mm");
                var fMaxima = fechaMaxima.ToString("yyy/MM/dd HH:mm");

                string query = "SELECT fechahora, potencia, angulo " +
                               "FROM datos " +
                               "WHERE fechahora >= '" + fMinima + "' AND fechahora <= '" + fMaxima + "'";

                MySqlCommand cmd = new MySqlCommand(query, conexion);

                conexion.Open();

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new ListaDatosTiempo() {

                            Fecha = dr["fechahora"].ToString(),
                            Potencia = double.Parse(dr["potencia"].ToString()),
                            Angulo = double.Parse(dr["angulo"].ToString())
                        });
                    }
                }
            }

            return objLista;
        }

     }

 }
