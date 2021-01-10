using AdresBeheerOpdracht.Exceptions;
using System;
using System.Collections.Generic;

namespace AdresBeheerOpdracht.Model
{
    public class Bestelling : Observable
    {
        public int BestellingId { get; private set; }
        public bool Betaald { get; private set; }
        public double PrijsBetaald { get; private set; }
        public Klant Klant { get; set; }
        public DateTime Tijdstip { get; private set; }
        private Dictionary<Product, int> _producten = new Dictionary<Product, int>();

        public Bestelling(int bestellingId, DateTime tijdstip)
        {
            ZetBestellingId(bestellingId);
            ZetTijdstip(tijdstip);
            Betaald = false;
        }
        public Bestelling(int bestellingId, Klant klant, DateTime tijdstip) : this(bestellingId,tijdstip)
        {           
            ZetKlant(klant);
        }
        public Bestelling(int bestellingId, Klant klant, DateTime tijdstip, Dictionary<Product, int> producten) : this(bestellingId, klant, tijdstip)
        {
            if (producten is null) throw new BestellingException("producten zijn leeg");
            _producten = producten;
        }

        public void VoegProductToe(Product product, int aantal)
        {
            if (aantal <= 0) throw new BestellingException("VoegProductToe - aantal");
            if (_producten.ContainsKey(product))
            {
                _producten[product] += aantal;
            }
            else
            {
                _producten.Add(product, aantal);
            }
        }
        public void VerwijderProduct(Product product, int aantal)
        {
            if (aantal <= 0) throw new BestellingException("VerwijderProduct - aantal");
            if (!_producten.ContainsKey(product))
            {
                throw new BestellingException("VerwijderProduct - product niet beschikbaar");
            }
            else
            {
                if (_producten[product] < aantal)
                {
                    throw new BestellingException("VerwijderProduct - beschikbaar aantal te klein");
                }
                else if (_producten[product] == aantal)
                {
                    _producten.Remove(product);
                }
                else
                {
                    _producten[product] -= aantal;
                }
            }
        }
        public IReadOnlyDictionary<Product, int> GeefProducten()
        {
            return _producten;
        }
        public double Kostprijs() //procent
        {
            double prijs = 0.0;
            int korting;
            if (Klant is null)
            {
                korting = 0;
            }
            else
            {
                korting = Klant.Korting();
            }
            foreach (KeyValuePair<Product, int> kvp in _producten)
            {
                prijs += kvp.Key.Prijs * kvp.Value * (100.0 - korting) / 100.0;
            }
            NotifyPropertyChanged("Prijs");
            return prijs;
        }
        public void VerwijderKlant()
        {
            Klant = null;
        }
        public void ZetKlant(Klant newKlant)
        {
            if (newKlant == null) throw new BestellingException("Bestelling - invalid klant");
            if (newKlant == Klant) throw new BestellingException("Bestelling - ZetKlant - not new");
            if (Klant!=null)
                if (Klant.HeeftBestelling(this))
                    Klant.VerwijderBestelling(this);
            if (!newKlant.HeeftBestelling(this)) newKlant.VoegToeBestelling(this);
            Klant = newKlant;
        }
        public void ZetBestellingId(int id)
        {
            //hier werd fout gegooid omdat we in de WPF een nieuwe bestelling op 0 zetten aangezien de db not niet werkt
            //Hieronder vind je de bestelling id regel indien db gebruikt wordt
            //if (id <= 0) throw new BestellingException("Bestelling - invalid id");
            if(id < 0 ) throw new BestellingException("Bestelling - invalid id");
            BestellingId = id;
        }
        public void ZetTijdstip(DateTime tijdstip)
        {
            if (tijdstip == null) throw new BestellingException("Bestelling - invalid tijdstip");
            Tijdstip = tijdstip;
        }
        public void ZetBetaald(bool betaald = true)
        {
            Betaald = betaald;
            if (betaald)
            {
                PrijsBetaald = Kostprijs();
            }
            NotifyPropertyChanged("Betaald");
        }

        public override bool Equals(object obj)
        {
            return obj is Bestelling bestelling &&
                   BestellingId == bestelling.BestellingId;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(BestellingId);
        }
        public override string ToString()
        {
            return $"[Bestelling] {BestellingId},{Betaald},{PrijsBetaald},{Tijdstip},{Klant},{_producten.Count}";
        }
        public void Show()
        {
            Console.WriteLine(this);
            foreach (KeyValuePair<Product,int> kvp in _producten) 
                Console.WriteLine($"    product:{kvp.Key},{kvp.Value}");
        }
    }
}