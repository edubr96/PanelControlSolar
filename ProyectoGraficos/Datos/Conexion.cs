using MySql.Data.MySqlClient;

namespace PanelControlSolar.Datos
{
    public class Conexion
    {
        public MySqlConnection mySqlConnection { get; set; }

        public Conexion ()
        {
            mySqlConnection = new MySqlConnection("Server=db4free.net;Port=3306;Database=panelsolar;Uid=edub96;Pwd=628605010;");
        }
    }
}