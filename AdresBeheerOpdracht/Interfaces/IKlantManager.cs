using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;

namespace AdresBeheerOpdracht.Interfaces
{
    public interface IKlantManager
    {
        Klant GeefKlant(int klantId);
        IReadOnlyList<Klant> GeefKlanten();
        IReadOnlyList<Klant> GeefKlanten(Func<Klant, bool> predicate);
        void VerwijderKlant(Klant klant);
        void VoegKlantToe(Klant klant);
    }
}