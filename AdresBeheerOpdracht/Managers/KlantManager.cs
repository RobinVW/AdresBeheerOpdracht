using AdresBeheerOpdracht.Exceptions;
using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdresBeheerOpdracht.Managers
{
    public class KlantManager
    {
        private Dictionary<int, Klant> _klanten = new Dictionary<int, Klant>();
        public IReadOnlyList<Klant> GeefKlanten()
        {
            return new List<Klant>(_klanten.Values).AsReadOnly();
        }
        public void VoegKlantToe(Klant klant)
        {
            if (_klanten.ContainsKey(klant.KlantId))
            {
                throw new KlantManagerException("VoegKlantToe");
            }
            else
            {
                _klanten.Add(klant.KlantId, klant);
            }
        }
        public void VerwijderKlant(Klant klant)
        {
            if (!_klanten.ContainsKey(klant.KlantId))
            {
                throw new KlantManagerException("VerwijderKlant");
            }
            else
            {
                _klanten.Remove(klant.KlantId);
            }
        }
        public Klant GeefKlant(int klantId)
        {
            if (!_klanten.ContainsKey(klantId))
            {
                throw new KlantManagerException("GeefKlant");
            }
            else
            {
                return _klanten[klantId];
            }
        }
    }
}
