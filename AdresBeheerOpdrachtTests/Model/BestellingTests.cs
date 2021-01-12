using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;
using System.Text;
using AdresBeheerOpdracht.Exceptions;

namespace AdresBeheerOpdracht.Model.Tests
{
    [TestClass()]
    public class BestellingTests
    {
        [TestMethod()]
        public void Test_CTR_idTijdstip()
        {
            int id = 5;
            DateTime dt = DateTime.Now;
            Bestelling b = new Bestelling(id, dt);

            Assert.AreEqual(dt, b.Tijdstip);
            Assert.AreEqual(id, b.BestellingId);
            Assert.AreEqual(default(Boolean), b.Betaald);
        }

        [TestMethod()]
        public void Test_CTR_idTijdstipKlant()
        {
            int id = 5;
            DateTime dt = DateTime.Now;
            Klant k = new Klant("robin", "adres ofwa");
            Bestelling b = new Bestelling(id,k, dt);

            Assert.AreEqual(dt, b.Tijdstip);
            Assert.AreEqual(id, b.BestellingId);
            Assert.AreEqual(default(Boolean), b.Betaald);
            Assert.AreEqual(k, b.Klant);
        }

        [TestMethod()]
        public void Test_VerwijderKlant()
        {
            int id = 5;
            DateTime dt = DateTime.Now;
            Klant k = new Klant("robin", "adres ofwa");
            Bestelling b = new Bestelling(id, k, dt);

            b.VerwijderKlant();

            Assert.AreEqual(null, b.Klant);
        }

        [TestMethod()]
        public void Test_ZetID_LessThenZero()
        {
            int id = 5;
            DateTime dt = DateTime.Now;
            Klant k = new Klant("robin", "adres ofwa");
            Bestelling b = new Bestelling(id, k, dt);


            Assert.ThrowsException<BestellingException>(() => b.ZetBestellingId(-1));
        }

        [TestMethod]
        public void Test_ZetKlant()
        {
            int id = 5;
            DateTime dt = DateTime.Now;
            Klant k = new Klant("robin", "adres ofwa");
            Bestelling b = new Bestelling(id, k, dt);

            Klant newK = new Klant("jean", "nog een adres ofwa");
            b.ZetKlant(newK);

            Assert.AreEqual(newK, b.Klant);
        }

        [TestMethod]
        public void Test_ZetBetaald()
        {
            int id = 5;
            DateTime dt = DateTime.Now;
            Klant k = new Klant("robin", "adres ofwa");
            Bestelling b = new Bestelling(id, k, dt);

            b.ZetBetaald();

            Assert.AreEqual(true, b.Betaald);
        }
    }
}