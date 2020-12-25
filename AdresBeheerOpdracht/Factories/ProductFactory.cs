using AdresBeheerOpdracht.Exceptions;
using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdresBeheerOpdracht.Tools
{
    public static class ProductFactory
    {
        public static Product MaakProduct(string naam,double prijs,IDFactory idFactory)
        {
            try
            {
                return new Product(idFactory.MaakProductID(), naam.Trim(), prijs);
            }
            catch(ProductException ex)
            {
                throw new ProductFactoryException("MaakProduct", ex);
            }
        }
    }
}
