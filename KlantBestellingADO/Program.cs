using AdresBeheerOpdracht.Interfaces;
using AdresBeheerOpdracht.Model;
using KlantBestellingADO.Managers;
using System;
using System.Collections.Generic;

namespace KlantBestellingADO2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Klant klant1 = new Klant("Robin Van Welden", "Kortrijkstraat 60 9700 Oudenaarde");
            Klant klant2 = new Klant("Jean Bosmans", "Bostraat 50 9000 Gent");
            Klant klant3 = new Klant("Jan Aap", "eenstraatinbelgie 50 9000 Gent");
            Klant klant4 = new Klant("Joske Vermeulen", "teststraat 4 9700 Oudenaarde");*/

            IKlantManager km = new KlantManagerSQL("Data Source=DESKTOP-ICIHEO8\\SQLEXPRESS;Initial Catalog=KlantBestellingADO;Integrated Security=True;MultipleActiveResultSets=True");

            /*km.VoegKlantToe(klant1);
            km.VoegKlantToe(klant2);

            foreach (var k in km.GeefKlanten())
            {
                k.Show();
            }
            Console.WriteLine("klant 1 & 2 toegevoegd\n");

            km.VoegKlantToe(klant3);

            foreach (var k in km.GeefKlanten())
            {
                k.Show();
            }
            Console.WriteLine("klant 3 toegevoegd\n");

            km.VerwijderKlant(klant2);

            foreach (var k in km.GeefKlanten())
            {
                k.Show();
            }
            Console.WriteLine("klant 2 verwijderd\n");

            km.VoegKlantToe(klant4);
            km.GeefKlant(4).Show();
            Console.WriteLine("klant 4 toevoegen + showen\n");

            Product product1 = new Product("Lorazepam", 4.50);
            Product product2 = new Product("Goats Milk", 8.50);
            Product product3 = new Product("CLENZIDERM THERAPEUTIC", 1.50);
            Product product4 = new Product("Prevacid SoluTab", 5.50);
            Product product5 = new Product("Ponstel", 4.50);
            Product product6 = new Product("IBANDRONATE SODIUM", 7.50);
            Product product7 = new Product("Cefdinir", 8.50);
            Product product8 = new Product("Bumetanide", 1.50); */

            IProductManager pm = new ProductManagerSQL("Data Source=DESKTOP-ICIHEO8\\SQLEXPRESS;Initial Catalog=KlantBestellingADO;Integrated Security=True");

            /*pm.VoegProductToe(product1);
            pm.VoegProductToe(product2);
            pm.VoegProductToe(product3);
            pm.VoegProductToe(product4);
            pm.VoegProductToe(product5);
            pm.VoegProductToe(product6);
            pm.VoegProductToe(product7);
            pm.VoegProductToe(product8);

            foreach(var p in pm.GeefProducten())
            {
                Console.WriteLine(p.ToString());
            }
            Console.WriteLine(pm.GeefProducten().Count + " producten toegevoegd \n");

            pm.VerwijderProduct(product4);
            foreach (var p in pm.GeefProducten())
            {
                Console.WriteLine(p.ToString());
            }
            Console.WriteLine("product4 verwijderd \n");

            Console.WriteLine("selecteer product 1 op basis van ID:");
            Console.WriteLine(pm.GeefProduct(1).ToString()+"\n");

            Console.WriteLine("selecteer product 3 op basis van Naam:");
            Console.WriteLine(pm.GeefProduct(product3.Naam).ToString() + "\n");*/

            IBestellingManager bm = new BestellingManagerSQL("Data Source=DESKTOP-ICIHEO8\\SQLEXPRESS;Initial Catalog=KlantBestellingADO;Integrated Security=True;MultipleActiveResultSets=True");

            /*Dictionary<Product, int> _producten = new Dictionary<Product, int>();
            _producten.Add(pm.GeefProduct(1), 1);
            _producten.Add(pm.GeefProduct(2), 2);
            _producten.Add(pm.GeefProduct(3), 1);
            _producten.Add(pm.GeefProduct(5), 5);

            Bestelling bestelling = new Bestelling(0, km.GeefKlant(1), DateTime.Now, _producten);
            bestelling.ZetBetaald();

            bm.VoegBestellingToe(bestelling);*/
            //bm.GeefBestelling(1).Show();
            //bm.VerwijderBestelling(bm.GeefBestelling(2));

            /*foreach (var b in bm.GeefBestellingen())
            {
                b.Show();
            }*/

            /*foreach (var k in km.GeefKlanten())
            {
                foreach (var b in k.GetBestellingen())
                {
                    Console.WriteLine("geef klanten -> alle bestellingen -> show");
                    b.Show();
                }
            }*/
            /*bm.GeefBestellingen();
            
            foreach (var b in bm.GeefBestellingen(b => b.Klant.KlantId == 1))
            {
                b.Show();
            }*/
            
            
        }
    }
}

