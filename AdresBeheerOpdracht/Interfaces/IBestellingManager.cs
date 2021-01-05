using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;

namespace AdresBeheerOpdracht.Interfaces
{
    public interface IBestellingManager
    {
        Bestelling GeefBestelling(int bestellingId);
        IReadOnlyList<Bestelling> GeefBestellingen();
        IReadOnlyList<Bestelling> GeefBestellingen(Func<Bestelling, bool> predicate);
        void VerwijderBestelling(Bestelling bestelling);
        void VoegBestellingToe(Bestelling bestelling);
    }
}