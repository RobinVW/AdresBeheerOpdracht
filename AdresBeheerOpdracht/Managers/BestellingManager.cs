using AdresBeheerOpdracht.Exceptions;
using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdresBeheerOpdracht.Managers
{
    public class BestellingManager
    {
        private Dictionary<int, Bestelling> _bestellingen = new Dictionary<int, Bestelling>();
        public IReadOnlyList<Bestelling> GeefBestellingen()
        {
            return new List<Bestelling>(_bestellingen.Values).AsReadOnly();
        }

        public IReadOnlyList<Bestelling> GeefBestellingen(Func<Bestelling, bool> predicate)
        {
            var selection = _bestellingen.Values.Where<Bestelling>(predicate).ToList();
            return (IReadOnlyList<Bestelling>)selection;
        }

        public void VoegBestellingToe(Bestelling bestelling)
        {
            if (_bestellingen.ContainsKey(bestelling.BestellingId))
            {
                throw new BestellingManagerException("VoegBestellingToe");
            }
            else
            {
                _bestellingen.Add(bestelling.BestellingId, bestelling);
            }
        }
        public void VerwijderBestelling(Bestelling bestelling)
        {
            if (!_bestellingen.ContainsKey(bestelling.BestellingId))
            {
                throw new BestellingManagerException("VerwijderBestelling");
            }
            else
            {
                _bestellingen.Remove(bestelling.BestellingId);
            }
        }
        public Bestelling GeefBestelling(int bestellingId)
        {
            if (!_bestellingen.ContainsKey(bestellingId))
            {
                throw new BestellingManagerException("GeefBestelling");
            }
            else
            {
                return _bestellingen[bestellingId];
            }
        }
    }
}
