using AdresBeheerOpdracht.Interfaces;
using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace KlantBestellingADO.Managers
{
    public class KlantManagerSQL : IKlantManager
    {
        private string connectionString;

        private Dictionary<int, Klant> _klanten = new Dictionary<int, Klant>();

        public KlantManagerSQL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public Klant GeefKlant(int klantId)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.klant WHERE klantID = @id";
            using(SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.Add("@id", SqlDbType.Int).Value = klantId;
                connection.Open();
                try
                {
                    IDataReader reader = command.ExecuteReader();
                    reader.Read();
                    Klant klant = new Klant((int)reader["klantID"], (string)reader["naam"], (string)reader["adres"]);
                    reader.Close();
                    return klant;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IReadOnlyList<Klant> GeefKlanten()
        {
            //maak de klanten dictionary leeg aangezien hij toch gevult wordt door de db.
            _klanten.Clear();
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.klant";
            using(SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int id = (int)dataReader["klantId"];
                        string naam = dataReader.GetString(dataReader.GetOrdinal("naam"));
                        string adres = dataReader.GetString(dataReader.GetOrdinal("adres"));
                        _klanten.Add(id, new Klant(id, naam, adres));
                    }
                    dataReader.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
            return new List<Klant>(_klanten.Values).AsReadOnly();
        }

        public IReadOnlyList<Klant> GeefKlanten(Func<Klant, bool> predicate)
        {
            //Initialiseer alle klanten in de _klanten dictionary
            GeefKlanten();
            var selection = _klanten.Values.Where<Klant>(predicate).ToList();
            return (IReadOnlyList<Klant>)selection;
        }

        public void VerwijderKlant(Klant klant)
        {
            SqlConnection connection = getConnection();
            string query = "DELETE FROM dbo.klant WHERE naam = @naam AND adres = @adres";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add("@naam", SqlDbType.NVarChar, 50).Value = klant.Naam;
                    command.Parameters.Add("@adres", SqlDbType.NVarChar, 400).Value = klant.Adres;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void VoegKlantToe(Klant klant)
        {
            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.klant (naam,adres) VALUES (@naam,@adres)";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add("@naam", SqlDbType.NVarChar, 50).Value = klant.Naam;
                    command.Parameters.Add("@adres", SqlDbType.NVarChar, 400).Value = klant.Adres;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            /*//checken of klantId al in de lijst zit, zoniet toevoegen aan de lijst
            if (!_klanten.ContainsKey(klant.KlantId))
            {
                _klanten.Add(klant.KlantId, klant);
            }*/
        }
    }
}
