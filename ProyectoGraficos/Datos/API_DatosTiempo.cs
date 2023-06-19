using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;

namespace PanelControlSolar.Datos
{
    public class API_DatosTiempo
    {
        private const string APPId = "fcee5cd1";
        private const string APIKey = "4b2196ddacfbf97a334790e2003f7605";
        public static string temperaturaActual;
        public void DatosAPI()
        {
            //Inyectamos el CP obtenido de la base de datos
            string weatherData = GetWeatherData(CodigoPostal.Codigopostal);

            // Procesar los datos del clima
            JObject json = JObject.Parse(weatherData);
            temperaturaActual = json["temp_c"].ToString();
        }

        private static string GetWeatherData(string zipCode)
        {
            string apiUrl = $"http://api.weatherunlocked.com/api/current/es.{zipCode}?app_id={APPId}&app_key={APIKey}";

            try
            {
                WebRequest request = WebRequest.Create(apiUrl);
                using (WebResponse response = request.GetResponse())
                using (Stream dataStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del clima: {0}", ex.Message);
                return null;
            }
        }

        private static string GetWeatherDataPorFecha(string zipCode, DateTime fecha)
        {
            string fechaFormateada = fecha.ToString("yyyy.MM.dd");

            //Limitacion api. Usaremos el dia actual.
            string apiUrl = $"http://api.weatherunlocked.com/api/trigger/es.{zipCode}/forecast {fechaFormateada} ?app_id={APPId}&app_key={APIKey}";
            apiUrl = $"http://api.weatherunlocked.com/api/current/es.{zipCode}?app_id={APPId}&app_key={APIKey}";

            try
            {
                WebRequest request = WebRequest.Create(apiUrl);
                using (WebResponse response = request.GetResponse())
                using (Stream dataStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del clima para la fecha {0}: {1}", fecha.ToString("dd/MM/yyyy"), ex.Message);
                return null;
            }
        }

        public string[] ObtenerTemperaturasRangoFechas(string[] fechas)
        {
            // Crear un array para almacenar las temperaturas
            string[] temperaturas = new string[fechas.Length];

            bool sumaGrado = false;

            // Obtener las temperaturas para cada día en el rango especificado
            for (int i = 0; i < fechas.Length; i++)
            {
                
                int temp = Convert.ToInt32(temperaturaActual);

                if (sumaGrado)
                {
                    temp += 1;
                    sumaGrado = false;
                }
                else { 
                    
                    temp -= 1; 
                    sumaGrado = true;
                }

                // Almacenar la temperatura en el array
                temperaturas[i] = temp.ToString();
            }

            // Devolver el array de temperaturas
            return temperaturas;
        }
    }

   
}