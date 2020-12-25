using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;
using System.Text;
using AdresBeheerOpdracht.Exceptions;
using System.Linq;

namespace AdresBeheerOpdracht.Managers
{
    public class ProductManager
    {
        private Dictionary<string, Product> _producten = new Dictionary<string, Product>();
        public IReadOnlyList<Product> GeefProducten()
        {
            return new List<Product>(_producten.Values).AsReadOnly();
        }
        public void VoegProductToe(Product product)
        {
            try
            {
                if (_producten.ContainsKey(product.Naam))
                {
                    throw new ProductManagerException("VoegProductToe");
                }
                else
                {
                    _producten.Add(product.Naam, product);
                }
            }
            catch(Exception ex)
            {
                throw new ProductManagerException("VoegProductToe", ex);
            }
        }
        public void VerwijderProduct(Product product)
        {
            if (!_producten.ContainsKey(product.Naam))
            {
                throw new ProductManagerException("VerwijderProduct");
            }
            else
            {
                _producten.Remove(product.Naam);
            }
        }
        public Product GeefProduct(string naam)
        {
            if (!_producten.ContainsKey(naam))
            {
                throw new ProductManagerException("GeefProduct");
            }
            else
            {
                return _producten[naam];
            }
        }
        public Product GeefProduct(int productId)
        {
            if (!_producten.Values.Any(x=>x.ProductId==productId))
            {
                throw new ProductManagerException("GeefProduct");
            }
            else
            {
                return _producten.Values.First(x => x.ProductId == productId);
            }
        }
    } 
}
