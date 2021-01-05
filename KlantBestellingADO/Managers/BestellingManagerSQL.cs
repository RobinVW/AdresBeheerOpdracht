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
            throw new NotImplementedException();
        }

        public IReadOnlyList<Bestelling> GeefBestellingen()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Bestelling> GeefBestellingen(Func<Bestelling, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void VerwijderBestelling(Bestelling bestelling)
        {
            throw new NotImplementedException();
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
