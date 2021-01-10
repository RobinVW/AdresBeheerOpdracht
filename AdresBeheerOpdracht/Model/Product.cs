using AdresBeheerOpdracht.Exceptions;
using System;

namespace AdresBeheerOpdracht.Model
{
    public class Product : Observable
    {
        public Product(string naam) => ZetNaam(naam);
        public Product(string naam, double prijs) : this(naam) => ZetPrijs(prijs);
        public Product(int productId, string naam, double prijs) : this(naam, prijs)
        {
            ZetProductId(productId);
        }
        public int ProductId { get; private set; }
        public string Naam { get; private set; }
        public double Prijs { get; private set; }
        public void ZetPrijs(double prijs)
        {
            if (prijs <= 0) throw new ProductException("Product prijs invalid");
            Prijs = prijs;
        }
        public void ZetNaam(string naam)
        {
            if (naam.Trim().Length < 1) throw new ProductException("Product naam invalid");
            Naam = naam;
        }
        public void ZetProductId(int productId)
        {
            if (productId <= 0) throw new ProductException("ProductId invalid");
            ProductId = productId;
        }

      
        public override string ToString()
        {
            return $"[Product] {ProductId},{Naam},{Prijs}";
        }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Naam == product.Naam;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Naam);
        }
    }
}
