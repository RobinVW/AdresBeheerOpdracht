using AdresBeheerOpdracht.Managers;
using AdresBeheerOpdracht.Model;
using AdresBeheerOpdracht.Tools;
using System;

namespace AdresBeheerOpdrachtConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IDFactory idF = new IDFactory(0, 100, 5000); //klant,bestelling,product
            var kM = new /*Db*/KlantManager();
            var pM = new ProductManager();
            var bM = new BestellingManager();

            pM.VoegProductToe(ProductFactory.MaakProduct("product 1", 10.0, idF));
            pM.VoegProductToe(ProductFactory.MaakProduct("product 2", 12.0, idF));
            pM.VoegProductToe(ProductFactory.MaakProduct("product 3", 13.0, idF));
            foreach (var x in pM.GeefProducten()) Console.WriteLine(x);

            kM.VoegKlantToe(KlantFactory.MaakKlant("klant 1", "adres 1", idF));
            kM.VoegKlantToe(KlantFactory.MaakKlant("klant 2", "adres 2", idF));
            foreach (var x in kM.GeefKlanten()) //Console.WriteLine(x);
                x.Show();

            bM.VoegBestellingToe(BestellingFactory.MaakBestelling(idF));
            bM.VoegBestellingToe(BestellingFactory.MaakBestelling(kM.GeefKlant(1), idF));

            Bestelling b = bM.GeefBestelling(101);
            b.VoegProductToe(pM.GeefProduct("product 2"),8);
            b.VoegProductToe(pM.GeefProduct("product 1"), 7);
            Console.WriteLine($"Prijs:{b.Kostprijs()}, {b.PrijsBetaald}");
            b.ZetBetaald();
            Console.WriteLine($"Prijs:{b.Kostprijs()}, {b.PrijsBetaald}");

            foreach (var x in bM.GeefBestellingen()) //Console.WriteLine(x);
                x.Show();
            foreach (var x in kM.GeefKlanten()) //Console.WriteLine(x);
                x.Show();
            Console.WriteLine("-----------------");
            Klant k1 = kM.GeefKlant(1);
            k1.Show();
            k1.VoegToeBestelling(b);
            k1.Show();
        }
    }
}

    


