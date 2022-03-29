using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BowlingPlayerFactoryShould
    {
        
        [Test]
        public void CreateTwoplayers()
        {
            BowlingPlayerFactory playerFactory = new BowlingPlayerFactory();
            IBowlingPlayer[] players = playerFactory.Create("German", "Leandro");
            Assert.AreEqual(2,players.Length);
        }

    }
}
