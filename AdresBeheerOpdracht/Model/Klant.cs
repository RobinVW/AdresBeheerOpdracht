using AdresBeheerOpdracht.Exceptions;
using System;
using System.Collections.Generic;

namespace AdresBeheerOpdracht.Model
{
    public class Klant
    {
        public int KlantId { get; private set; }
        public string Naam { get; private set; }
        public string Adres { get; private set; }
        private List<Bestelling> _bestellingen = new List<Bestelling>();

        public Klant(string naam, string adres)
        {
            ZetNaam(naam);
            ZetAdres(adres);
        }
        public Klant(int klantId, string naam, string adres, List<Bestelling> bestellingen) : this(klantId,naam, adres)
        {
            if (bestellingen == null) throw new KlantException("Klant - bestellingen null");
            _bestellingen = bestellingen;
            foreach (Bestelling b in bestellingen) b.ZetKlant(this);
        }
        public Klant(int klantId, string naam, string adres) : this(naam,adres)
        {
            KlantId = klantId;
        }
       
        public void ZetNaam(string naam)
        {
            if (naam.Trim().Length < 1) throw new KlantException("Klant naam invalid");
            Naam = naam;
        }
        public void ZetAdres(string adres)
        {
            if (adres.Trim().Length < 5) throw new KlantException("Klant adres invalid");
            Adres = adres;
        }
        public IReadOnlyList<Bestelling> GetBestellingen()
        {
            return _bestellingen.AsReadOnly();
        }
        public void VerwijderBestelling(Bestelling bestelling)
        {
            if (!_bestellingen.Contains(bestelling))
            {
                throw new KlantException("Klant : RemoveBestelling - bestelling does not exists");
            }
            else
            {
                if (bestelling.Klant==this)
                    bestelling.VerwijderKlant();
                _bestellingen.Remove(bestelling);
            }
        }
        public void VoegToeBestelling(Bestelling bestelling)
        {
            if (_bestellingen.Contains(bestelling))
            {
                throw new KlantException("Klant : AddBestelling - bestelling already exists");
            }
            else
            {
                _bestellingen.Add(bestelling);
                if (bestelling.Klant!=this)
                    bestelling.ZetKlant(this);
            }
        }
        public bool HeeftBestelling(Bestelling bestelling)
        {
            if (_bestellingen.Contains(bestelling)) return true;
            else return false;
        }        
        public int Korting() //procent
        {
            if (_bestellingen.Count < 5) return 0;
            if (_bestellingen.Count < 10) return 10;
            else return 20;
        }
        public override string ToString()
        {
            return $"[Klant] {KlantId},{Naam},{Adres},{_bestellingen.Count}";
        }
        public void Show()
        {
            Console.WriteLine(this);
            foreach (Bestelling b in _bestellingen) Console.WriteLine($"    bestelling:{b}");
        }
    }
}
