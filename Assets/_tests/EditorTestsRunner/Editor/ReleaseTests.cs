using Antura.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace Antura.Tests.Release
{
    [TestFixture]
    [Category("Antura Release")]
    internal class ReleaseTests
    {
        [Test]
        public void CheckAppConstants()
        {
            var failed = false;
            if (AppConfig.DebugLogEnabled) {
                UnityEngine.Debug.Log("DebugLogEnabled should be FALSE");
                failed = true;
            }

            if (AppConfig.OnlineAnalyticsEnabled == false) {
                UnityEngine.Debug.Log("UnityAnalyticsEnabled should be TRUE");
                failed = true;
            }

            if (AppConfig.DebugPanelEnabledAtStartup) {
                UnityEngine.Debug.Log("DebugPanelEnabledAtStartup should be FALSE");
                failed = true;
            }

            if (AppConfig.DebugLogDbInserts) {
                UnityEngine.Debug.Log("DebugLogDbInserts should be FALSE");
                failed = true;
            }

            if (AppConfig.DisableFirstContact) {
                UnityEngine.Debug.Log("DisableFirstContact should be FALSE");
                failed = true;
            }

            if (!AppConfig.MinigameTutorialsEnabled) {
                UnityEngine.Debug.Log("MinigameTutorialsEnabled should be TRUE");
                failed = true;
            }

            if (failed) {
                Assert.Fail();
            }
        }
    }
}