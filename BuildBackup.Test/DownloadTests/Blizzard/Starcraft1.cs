﻿using System.Diagnostics;
using System.Linq;
using BuildBackup.DebugUtil.Models;
using ByteSizeLib;
using Konsole;
using NUnit.Framework;

namespace BuildBackup.Test.DownloadTests.Blizzard
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class Starcraft1
    {
        private ComparisonResult _results;

        [OneTimeSetUp]
        public void Setup()
        {
            // Run the download process only once
            _results = Program.ProcessProduct(TactProducts.Starcraft1, new MockConsole(120, 50), true);
        }

        [Test]
        public void Misses()
        {
            Assert.LessOrEqual(0, _results.MissCount);
        }

        [Test]
        public void MissedBandwidth()
        {
            var missedBandwidth = ByteSize.FromBytes(_results.Misses.Sum(e => e.TotalBytes));
            Assert.AreEqual(missedBandwidth.Bytes, 0);
        }

        //TODO copy this to the rest of the tests
        [Test]
        public void TotalTime()
        {
            var expectedMilliseconds = 600;
            Assert.LessOrEqual(_results.ElapsedTime.TotalMilliseconds, expectedMilliseconds);
        }

        [Test]
        public void WastedBandwidth()
        {
            //TODO improve this
            var expected = ByteSize.FromMegaBytes(100);

            var wastedBandwidth = ByteSize.FromBytes(_results.UnnecessaryRequests.Sum(e => e.TotalBytes));
            Assert.Less(wastedBandwidth.Bytes, expected.Bytes);
        }
    }
}
