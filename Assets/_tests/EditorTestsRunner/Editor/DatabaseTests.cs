using System.Collections.Generic;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using NUnit.Framework;
using Antura.Language;

namespace Antura.Tests
{
    [TestFixture]
    public class DatabaseTests
    {
        [Test]
        public void QueryStaticDB()
        {
            var dbManager = new DatabaseManager(null, LanguageCode.arabic);
            dbManager.GetAllLetterData();
        }

        [Test]
        public void QueryDynamicDB()
        {
            var dbManager = new DatabaseManager(null, LanguageCode.arabic);
            dbManager.LoadDatabaseForPlayer("TEST");
            dbManager.FindLogInfoData(x => x.Timestamp > 1000);
        }

        [Test]
        public void InsertDynamicDB()
        {
            var dbManager = new DatabaseManager(null, LanguageCode.arabic);
            dbManager.LoadDatabaseForPlayer("TEST");
            var newLogInfoData = new LogInfoData();
            newLogInfoData.AppSession = GenericHelper.GetTimestampForNow();
            newLogInfoData.Timestamp = GenericHelper.GetTimestampForNow();
            newLogInfoData.Event = InfoEvent.Book;
            newLogInfoData.AdditionalData = "test:1";
            dbManager.Insert(newLogInfoData);
        }

    }
}
