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
            string klantQuery = "SELECT * FROM dbo.klant WHERE klantID = @klantID";
            string bestellingQuery = "Select * FROM dbo.bestelling b WHERE b.klantID = @klantID";
            string productQuery = "SELECT * FROM dbo.bestellingDetails bd"
                                    + " JOIN dbo.product p ON p.productID = bd.productID"
                                    + " WHERE bd.bestellingID = @bestellingID";

            using (SqlCommand command1 = connection.CreateCommand())
            using (SqlCommand command2 = connection.CreateCommand())
            using (SqlCommand command3 = connection.CreateCommand())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                command1.Transaction = transaction;
                command2.Transaction = transaction;
                command3.Transaction = transaction;
                try
                {
                    command1.CommandText = klantQuery;
                    command1.Parameters.Add(new SqlParameter("klantID", SqlDbType.Int));
                    command1.Parameters["klantID"].Value = klantId;
                    SqlDataReader klantReader = command1.ExecuteReader();
                    
                    command2.Parameters.Add(new SqlParameter("klantID", SqlDbType.Int));
                    command3.Parameters.Add(new SqlParameter("bestellingID", SqlDbType.Int));
                    klantReader.Read();
                    
                    Klant klant = new Klant((int)klantReader["klantID"], (string)klantReader["naam"], (string)klantReader["adres"]);
                    command2.Parameters["klantID"].Value = klant.KlantId;
                    command2.CommandText = bestellingQuery;
                    SqlDataReader bestellingReader = command2.ExecuteReader();
                    while (bestellingReader.Read())
                    {
                            Dictionary<Product, int> _producten = new Dictionary<Product, int>();
                            command3.Parameters["bestellingID"].Value = (int)bestellingReader["bestellingID"];
                            command3.CommandText = productQuery;
                            SqlDataReader productReader = command3.ExecuteReader();
                            while (productReader.Read())
                            {
                                _producten.Add(new Product((int)productReader["productID"], (string)productReader["naam"], (double)productReader["prijs"]), (int)productReader["aantal"]);
                            }
                            productReader.Close();
                            Bestelling bestelling = new Bestelling((int)bestellingReader["bestellingID"], klant, (DateTime)bestellingReader["tijdstip"], _producten);
                    }
                    bestellingReader.Close();
                    //_klanten.Add(klant.KlantId, klant);
                    
                    klantReader.Close();
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
            /*using(SqlCommand command = connection.CreateCommand())
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
            }*/

        }

        public IReadOnlyList<Klant> GeefKlanten()
        {
            //maak de klanten dictionary leeg aangezien hij toch gevult wordt door de db.
            _klanten.Clear();

            SqlConnection connection = getConnection();
            string klantQuery = "Select * FROM dbo.klant";
            string bestellingQuery = "Select * FROM dbo.bestelling b WHERE b.klantID = @klantID";
            string productQuery = "SELECT * FROM dbo.bestellingDetails bd"
                                    + " JOIN dbo.product p ON p.productID = bd.productID"
                                    + " WHERE bd.bestellingID = @bestellingID";
            using (SqlCommand command1 = connection.CreateCommand())
            using (SqlCommand command2 = connection.CreateCommand())
            using (SqlCommand command3 = connection.CreateCommand())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                command1.Transaction = transaction;
                command2.Transaction = transaction;
                command3.Transaction = transaction;
                try
                {
                    command1.CommandText = klantQuery;
                    SqlDataReader klantReader = command1.ExecuteReader();
                    command2.Parameters.Add(new SqlParameter("klantID", SqlDbType.Int));
                    command3.Parameters.Add(new SqlParameter("bestellingID", SqlDbType.Int));
                    while (klantReader.Read())
                    {
                        Klant klant = new Klant((int)klantReader["klantID"], (string)klantReader["naam"], (string)klantReader["adres"]);
                        command2.Parameters["klantID"].Value = klant.KlantId;
                        command2.CommandText = bestellingQuery;
                        SqlDataReader bestellingReader = command2.ExecuteReader();
                        while (bestellingReader.Read())
                        {
                            Dictionary<Product, int> _producten = new Dictionary<Product, int>();
                            command3.Parameters["bestellingID"].Value = (int)bestellingReader["bestellingID"];
                            command3.CommandText = productQuery;
                            SqlDataReader productReader = command3.ExecuteReader();
                            while (productReader.Read())
                            {
                                _producten.Add(new Product((int)productReader["productID"], (string)productReader["naam"], (double)productReader["prijs"]),(int)productReader["aantal"]);
                            }
                            productReader.Close();
                            Bestelling bestelling = new Bestelling((int)bestellingReader["bestellingID"], klant, (DateTime)bestellingReader["tijdstip"], _producten);
                        }
                        bestellingReader.Close();
                        _klanten.Add(klant.KlantId, klant);
                    }
                    klantReader.Close();
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
