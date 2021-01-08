using AdresBeheerOpdracht.Interfaces;
using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace KlantBestellingADO.Managers
{
    public class ProductManagerSQL : IProductManager
    {
        private string connectionString;
        private Dictionary<string, Product> _producten = new Dictionary<string, Product>();

        public ProductManagerSQL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public Product GeefProduct(int productId)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.product WHERE productID = @id";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.Add("@id", SqlDbType.Int).Value = productId;
                connection.Open();
                try
                {
                    IDataReader reader = command.ExecuteReader();
                    reader.Read();
                    Product product = new Product((int)reader["productID"], (string)reader["naam"], (double)reader["prijs"]);
                    reader.Close();
                    return product;
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

        public Product GeefProduct(string naam)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.product WHERE naam = @naam";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.Add("@naam", SqlDbType.NVarChar,50).Value = naam;
                connection.Open();
                try
                {
                    IDataReader reader = command.ExecuteReader();
                    reader.Read();
                    Product product = new Product((int)reader["productID"], (string)reader["naam"], (double)reader["prijs"]);
                    reader.Close();
                    return product;
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

        public IReadOnlyList<Product> GeefProducten()
        {
            //maak de producten dictionary leeg aangezien hij toch gevult wordt door de db.
            _producten.Clear();
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.product";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int id = (int)dataReader["productID"];
                        string naam = dataReader.GetString(dataReader.GetOrdinal("naam"));
                        double prijs = dataReader.GetDouble(dataReader.GetOrdinal("prijs"));
                        _producten.Add(naam, new Product(id, naam, prijs));
                    }
                    dataReader.Close();
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
            return new List<Product>(_producten.Values).AsReadOnly();
        }

        public void VerwijderProduct(Product product)
        {
            SqlConnection connection = getConnection();
            string query = "DELETE FROM dbo.product WHERE naam = @naam";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add("@naam", SqlDbType.NVarChar, 50).Value = product.Naam;
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

        public void VoegProductToe(Product product)
        {
            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.product (naam,prijs) VALUES (@naam,@prijs)";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add("@naam", SqlDbType.NVarChar, 50).Value = product.Naam;
                    command.Parameters.Add(new SqlParameter("@prijs", SqlDbType.Decimal)
                    {
                        Precision = 18,
                        Scale = 2
                    }).Value = product.Prijs;
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
    }
}
