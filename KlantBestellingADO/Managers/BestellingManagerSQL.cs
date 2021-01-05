using AdresBeheerOpdracht.Interfaces;
using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlantBestellingADO.Managers
{
    public class BestellingManagerSQL : IBestellingManager
    {
        public Bestelling GeefBestelling(int bestellingId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Bestelling> GeefBestellingen()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Bestelling> GeefBestellingen(Func<Bestelling, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void VerwijderBestelling(Bestelling bestelling)
        {
            throw new NotImplementedException();
        }

        public void VoegBestellingToe(Bestelling bestelling)
        {
            throw new NotImplementedException();
        }
    }
}
