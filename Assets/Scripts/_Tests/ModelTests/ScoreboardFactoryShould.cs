using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ScoreboardFactoryShould
    {
        
        [Test]
        public void CreateAscoreboardWithTwoPlayers()
        {
            ScoreboardFactory factory = new ScoreboardFactory();
            IScoreboard scoreboard = factory.CreateAscoreboard();
            Assert.IsNotNull(scoreboard);
            Assert.AreEqual(2, scoreboard.Players.Count);
        }

    }
}
