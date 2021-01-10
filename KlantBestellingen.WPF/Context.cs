// HOGENT 
using AdresBeheerOpdracht.Interfaces;
using AdresBeheerOpdracht.Managers;
using AdresBeheerOpdracht.Model;
using AdresBeheerOpdracht.Tools;
using KlantBestellingADO.Managers;
using System;
// Timer:
//using System;
//using System.Windows.Threading;

namespace KlantBestellingen.WPF
{
    public static class Context
    {
        #region Properties
        private static string connectionString = "Data Source=DESKTOP-ICIHEO8\\SQLEXPRESS;Initial Catalog=KlantBestellingADO;Integrated Security=True;MultipleActiveResultSets=True";
        public static IDFactory IdFactory { get; } = new IDFactory(0, 100, 5000);
        // DbKlantManager!
        public static IKlantManager KlantManager { get; } = new KlantManagerSQL(connectionString); // Experimenteer: kan ook nog altijd KlantManager zijn!
        public static IProductManager ProductManager { get; } = new ProductManagerSQL(connectionString);
        public static IBestellingManager BestellingManager { get; } = new BestellingManagerSQL(connectionString);
        #endregion

        // private static DispatcherTimer _timer; // is operationeel volledig los van WPF en de andere code

        public static void Populate()
        {

            // Test code: moet weg indien db opgevuld
            // ----------
            //KlantManager.VoegKlantToe(KlantFactory.MaakKlant("klant 1", "adres 1", IdFactory));
            //KlantManager.VoegKlantToe(KlantFactory.MaakKlant("klant 2", "adres 2", IdFactory));
            //DbProductManager dbProductMgr = new DbProductManager();

            var bestellingen = BestellingManager.GeefBestellingen();
            var klanten = KlantManager.GeefKlanten();
            var producten = ProductManager.GeefProducten();


           /* DbBestellingManager testDbOrderMgr = new DbBestellingManager();
            {
                var counter = 1;
                Bestelling bestelling = new Bestelling(0, DateTime.Now) { Klant = klanten[0] };
                foreach (var p in producten)
                {
                    bestelling.VoegProductToe(p, counter++);
                }
                testDbOrderMgr.VoegToe(bestelling);
            }*/
            

            /*
            // Test code: we initialiseren een timer die elke 10 seconden het adres aanpast - alsof dit op de business layer gebeurt
            // ----------
            _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(10) }; // timer loopt af om de 10 seconden
            _timer.Tick += _timer_Tick; // voer method uit wanner timer afloopt
            _timer.Start();
            */
        }

        /*
        private static int _counter = 0;
        private static void _timer_Tick(object sender, EventArgs e)
        {
            // We passen aan op de business layer, maar owv INotify... volgt WPF de aanpassingen
            foreach (var klant in KlantManager.GeefKlanten())
            {
                ++_counter;
                klant.ZetAdres(klant.Adres + _counter.ToString());
            }
        }
        */
    }
}