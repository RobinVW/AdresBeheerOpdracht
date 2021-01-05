using AdresBeheerOpdracht.Model;
using System.Collections.Generic;

namespace AdresBeheerOpdracht.Interfaces
{
    public interface IProductManager
    {
        Product GeefProduct(int productId);
        Product GeefProduct(string naam);
        IReadOnlyList<Product> GeefProducten();
        void VerwijderProduct(Product product);
        void VoegProductToe(Product product);
    }
}