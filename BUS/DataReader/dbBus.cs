using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace DataReader
{
    class dbBus
    {
        public string Errore { get; set; }
        public string StringaDiConnessione { get; }

        [Obsolete]
        public dbBus()
        {
            Errore = "";
            StringaDiConnessione = ConfigurationSettings.AppSettings["StringaDiConnessione"];
        }
        public List<int> RecuperaId()
        {
            List<int> ids = new List<int>();
            try
            {
                using (SqlConnection con = new SqlConnection(StringaDiConnessione))
                {
                    con.Open();

                    string query = "SELECT Id FROM info;";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int i = 0;

                        i = (int)reader["Id"];
                        ids.Add(i);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Errore = e.Message;
                Console.WriteLine(Errore);
            }
            return ids;
        }
        public bus RecuperaDati(int idBus)
        {
            bus b = new bus();
            try
            {
                using (SqlConnection con = new SqlConnection(StringaDiConnessione))
                {
                    con.Open();

                    string query = "SELECT * FROM info WHERE id = @id";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", idBus);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        b.id = 0;
                        b.id = (int)reader["Id"];
                        b.lat = (string)reader["LatPartenza"];
                        b.lon = (string)reader["LongPartenza"];
                        b.posti = (int)reader["nPosti"];
                        b.fermate = (int)reader["nFermate"]; 
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Errore = e.Message;
                Console.WriteLine(Errore);
            }
            return b;
        }
    }

}
