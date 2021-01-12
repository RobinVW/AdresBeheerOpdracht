using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdresBeheerOpdracht.Model;
using System;
using System.Collections.Generic;
using System.Text;
using AdresBeheerOpdracht.Exceptions;

namespace AdresBeheerOpdracht.Model.Tests
{
    [TestClass()]
    public class KlantTests
    {
        [TestMethod()]
        public void Test_CTR_NaamAdres()
        {
            string naam = "Robin";
            string adres = "kortrijkstraat 60 9700 oudenaarde";

            Klant k = new Klant(naam, adres);

            Assert.AreEqual(naam, k.Naam);
            Assert.AreEqual(adres, k.Adres);
            Assert.AreEqual(default(int), k.KlantId);
        }

        [TestMethod()]
        public void Test_CTR_NaamAdresId()
        {
            string naam = "Robin";
            string adres = "kortrijkstraat 60 9700 oudenaarde";
            int id = 100;

            Klant k = new Klant(id, naam, adres);

            Assert.AreEqual(naam, k.Naam);
            Assert.AreEqual(adres, k.Adres);
            Assert.AreEqual(id, k.KlantId);
        }

        [TestMethod()]
        public void Test_ZetNaam_Valid()
        {
            string naam = "robin";
            string newNaam = "andreas";
            string adres = "dit is een andreas";
            Klant k = new Klant(naam, adres);

            Assert.AreEqual(naam, k.Naam);
            Assert.AreEqual(adres, k.Adres);
            Assert.AreEqual(default(int), k.KlantId);

            k.ZetNaam(newNaam);

            Assert.AreEqual(newNaam, k.Naam);
            Assert.AreEqual(adres, k.Adres);
            Assert.AreEqual(default(int), k.KlantId);
        }

        [TestMethod()]
        public void ZetAdresTest()
        {
            string naam = "robin";
            string adres = "dit is een andreas";
            string adresNew = "dit is zijn nieuwe adres";
            Klant k = new Klant(naam, adres);

            Assert.AreEqual(naam, k.Naam);
            Assert.AreEqual(adres, k.Adres);
            Assert.AreEqual(default(int), k.KlantId);

            k.ZetAdres(adresNew);

            Assert.AreEqual(naam, k.Naam);
            Assert.AreEqual(adresNew, k.Adres);
            Assert.AreEqual(default(int), k.KlantId);
        }

        [TestMethod()]
        public void Test_ZetNaam_ThrowKlantException()
        {
            string naam = "robin";
            string naamFout = "";
            string adres = "robin zijn adres";
            Klant k = new Klant(naam, adres);

            Assert.ThrowsException<KlantException>(() => k.ZetNaam(naamFout));
        }

        [TestMethod()]
        public void Test_ZetAdres_ThrowKlantException()
        {
            string naam = "robin";
            string adres = "robin zijn adres";
            string adresFout = "";
            Klant k = new Klant(naam, adres);

            Assert.ThrowsException<KlantException>(() => k.ZetAdres(adresFout));
        }

        [TestMethod()]
        public void Test_CTR_Naam_ThrowKlantException()
        {
            string naam = "";
            string adres = "dit is een adres";
            Klant k;

            Assert.ThrowsException<KlantException>(()=> k = new Klant(naam,adres));
        }

        [TestMethod()]
        public void Test_CTR_Adres_ThrowKlantException()
        {
            string naam = "robin";
            string adres = "";
            Klant k;

            Assert.ThrowsException<KlantException>(() => k = new Klant(naam, adres));
        }
    }
}