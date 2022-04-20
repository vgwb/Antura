using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using Antura.Language;

namespace Antura.Tests
{
    [TestFixture]
    [Category("Antura Helpers")]
    internal class HelperTests
    {
        [Test]
        public void MathHelperGetAverage()
        {
            var floatList = new List<float> { 0.1f, 0.4f, 0.8f, 2.99f, -1.0f };
            var average = Helpers.MathHelper.GetAverage(floatList);
            UnityEngine.Debug.Log(average);
            Assert.AreEqual(0.657999992f, average);
        }

        //[Test]
        //public void PassingTest()
        //{
        //    Assert.Pass();
        //}

        [Test]
        public void ArabicHelper()
        {
            var HexCode = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetHexUnicodeFromChar('A');
            UnityEngine.Debug.Log("ArabicHelper hexcode is: " + HexCode);
            Assert.Pass();
        }

        [Test]
        public void ArabicStringTest()
        {
            var dbManager = new DatabaseManager(null, LanguageCode.arabic);

            var wordList = dbManager.FindWordData(x => x.Id == "color_brown");
            var word = wordList[0];
            UnityEngine.Debug.Log("ArabicStringTest word " + word.Id + " - " + word.ToString());
            Assert.Pass();
        }

    }
}
