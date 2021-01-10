using AdresBeheerOpdracht.Interfaces;
using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace KlantBestellingADO.Managers
{
    public class BestellingManagerSQL : IBestellingManager
    {
        private string connectionString;

        private Dictionary<int, Bestelling> _bestellingen = new Dictionary<int, Bestelling>();
        public BestellingManagerSQL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public Bestelling GeefBestelling(int bestellingId)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.bestelling WHERE bestellingID = @bestellingID";
            string query2 = "SELECT dbo.product.productID, dbo.product.naam, dbo.product.prijs, dbo.bestellingDetails.aantal FROM dbo.product"
                + " INNER JOIN dbo.bestellingDetails ON dbo.product.productID = dbo.bestellingDetails.productID"
                + " AND dbo.bestellingDetails.bestellingID = @bestellingID";
            string query3 = "SELECT dbo.klant.klantID, dbo.klant.naam, dbo.klant.adres FROM dbo.klant"
+ " INNER JOIN dbo.bestelling ON dbo.klant.klantID = dbo.bestelling.klantID AND dbo.bestelling.bestellingID = @bestellingID";
            using (SqlCommand command1 = connection.CreateCommand())
            using (SqlCommand command2 = connection.CreateCommand())
            using (SqlCommand command3 = connection.CreateCommand())
            {
                connection.Open();
                command1.Parameters.Add("bestellingID", SqlDbType.Int).Value = bestellingId;
                command2.Parameters.Add("bestellingID", SqlDbType.Int).Value = bestellingId;
                command3.Parameters.Add("bestellingID", SqlDbType.Int).Value = bestellingId;
                command1.CommandText = query;
                command2.CommandText = query2;
                command3.CommandText = query3;
                SqlTransaction transaction = connection.BeginTransaction();
                command1.Transaction = transaction;
                command2.Transaction = transaction;
                command3.Transaction = transaction;
                try
                {
                    
                    
                    IDataReader klantReader = command3.ExecuteReader();
                  
                    klantReader.Read();
                    Klant klant = new Klant((int)klantReader["klantID"], (string)klantReader["naam"], (string)klantReader["adres"]);
                    klantReader.Close();

                    IDataReader productenReader = command2.ExecuteReader();
                    Dictionary<Product, int> _producten = new Dictionary<Product, int>();
                    while (productenReader.Read()) {
                        Product product = new Product((int)productenReader["productID"], (string)productenReader["naam"], (double)productenReader["prijs"]);
                        _producten.Add(product, (int)productenReader["aantal"]);
                    }
                    productenReader.Close();

                    IDataReader bestellingReader = command1.ExecuteReader();
                    bestellingReader.Read();
                    Bestelling bestelling = new Bestelling((int)bestellingReader["bestellingID"], klant, (DateTime)bestellingReader["tijdstip"], _producten);
                    if ((Boolean)bestellingReader["betaald"]) {
                        bestelling.ZetBetaald();
                    }
                    bestellingReader.Close();
                    transaction.Commit();
                    return bestelling;
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

        public IReadOnlyList<Bestelling> GeefBestellingen()
        {
            _bestellingen.Clear();
            SqlConnection connection = getConnection();
            string bQuery = "SELECT * FROM dbo.bestelling b ORDER BY b.klantID";
            string pQuery = "SELECT p.*,bd.aantal FROM dbo.bestellingDetails bd"
                            +" JOIN dbo.product p ON p.productID = bd.productID"
                            +" WHERE bd.bestellingID = @bestellingID";
            string kQuery = "SELECT * FROM dbo.klant k WHERE k.klantID = @klantID";

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
                    command1.CommandText = bQuery;
                    SqlDataReader bReader = command1.ExecuteReader();
                    command2.Parameters.Add(new SqlParameter("bestellingID", SqlDbType.Int));
                    command3.Parameters.Add(new SqlParameter("klantID", SqlDbType.Int));

                    Klant bestaandeKlant = null;

                    while (bReader.Read())
                    {
                        Dictionary<Product, int> _producten = new Dictionary<Product, int>();
                        command2.Parameters["bestellingID"].Value = (int)bReader["bestellingID"];
                        command2.CommandText = pQuery;
                        SqlDataReader pReader = command2.ExecuteReader();
                        while (pReader.Read())
                        {
                            _producten.Add(new Product((int)pReader["productID"], (string)pReader["naam"], (double)pReader["prijs"]), (int)pReader["aantal"]);
                        }
                        pReader.Close();

                        command3.CommandText = kQuery;
                        command3.Parameters["klantID"].Value = (int)bReader["klantID"];
                        SqlDataReader kReader = command3.ExecuteReader();
                        kReader.Read();
                        Klant klant = new Klant((int)bReader["klantID"], (string)kReader["naam"], (string)kReader["adres"]);
                        kReader.Close();

                        //indien het de eerste keer door de while lus gaat wordt de klant gelink aan bestaande klant
                        if(bestaandeKlant == null) bestaandeKlant = klant;
                        //indien de klant niet gelijk is aan de bestaande klant wordt de bestaande klant aangepast, op die manier worden de bestellingen aan de lijst van 
                        //bestelling van de klant toegevoegd indien de klant dus al bestaat
                        if(!bestaandeKlant.Equals(klant)) bestaandeKlant = klant;

                        Bestelling bestelling = new Bestelling((int)bReader["bestellingID"], bestaandeKlant, (DateTime)bReader["tijdstip"], _producten);
                        //check of bestelling betaald is, zoja -> zet betaald
                        if ((Boolean)bReader["betaald"])
                        {
                            bestelling.ZetBetaald();
                        }
                        _bestellingen.Add(bestelling.BestellingId, bestelling);

                    }
                    bReader.Close();
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
            return new List<Bestelling>(_bestellingen.Values).AsReadOnly();
        }

        public IReadOnlyList<Bestelling> GeefBestellingen(Func<Bestelling, bool> predicate)
        {
            GeefBestellingen();
            var selection = _bestellingen.Values.Where<Bestelling>(predicate).ToList();
            return (IReadOnlyList<Bestelling>)selection;
        }

        public IReadOnlyList<Bestelling> GeefBestellingen(Klant klant)
        {
            var selection = _bestellingen.Values.Where(b => b.Klant == klant).ToList();
            return (IReadOnlyList<Bestelling>)selection;
        }

        public void VerwijderBestelling(Bestelling bestelling)
        {
            SqlConnection connection = getConnection();
            string query = "DELETE FROM dbo.bestellingDetails WHERE bestellingID = @bestellingID;"
            +" DELETE FROM dbo.bestelling WHERE bestellingID = @bestellingID";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                
                try
                {
                    command.Parameters.Add("@bestellingID", SqlDbType.Int).Value = bestelling.BestellingId;
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

        public void VoegBestellingToe(Bestelling bestelling)
        {
            if(bestelling.GeefProducten().Count() != 0)
            {
                SqlConnection connection = getConnection();
                string query = "INSERT INTO dbo.bestelling (betaald, prijsBetaald, tijdstip, klantID) OUTPUT INSERTED.bestellingID VALUES (@betaald,@prijsBetaald,@tijdstip,@klantID)";
                string query2 = "INSERT INTO dbo.bestellingDetails(productID,bestellingID,aantal) VALUES (@productID,@bestellingID,@aantal)";
                using (SqlCommand command1 = connection.CreateCommand())
                using (SqlCommand command2 = connection.CreateCommand())
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    command1.Transaction = transaction;
                    command2.Transaction = transaction;
                    try
                    {
                        //bestelling toevoegen
                        command1.Parameters.Add("@betaald", SqlDbType.Bit).Value = bestelling.Betaald;
                        command1.Parameters.Add("@prijsBetaald", SqlDbType.Float).Value = bestelling.PrijsBetaald;
                        command1.Parameters.Add("@tijdstip", SqlDbType.DateTime).Value = bestelling.Tijdstip;
                        if (bestelling.Klant != null)
                        {
                            command1.Parameters.Add("@klantID", SqlDbType.Int).Value = bestelling.Klant.KlantId;
                        }

                        command1.CommandText = query;
                        int newID = (int)command1.ExecuteScalar();
                        //producten toevoegen
                        command2.Parameters.Add(new SqlParameter("@productID", SqlDbType.Int));
                        command2.Parameters.Add(new SqlParameter("@aantal", SqlDbType.Int));
                        command2.Parameters.Add("@bestellingID", SqlDbType.Int).Value = newID;

                        command2.CommandText = query2;

                        foreach (KeyValuePair<Product,int> kvp in bestelling.GeefProducten())
                        {
                            command2.Parameters["@productID"].Value = kvp.Key.ProductId;
                            command2.Parameters["@aantal"].Value = kvp.Value;
                            command2.ExecuteNonQuery();
                        }
                        transaction.Commit();
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
        }
    }
}
