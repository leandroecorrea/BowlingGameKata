using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerShould
    {
        [Test]
        public void HaveTenTurns()
        {
            // Given
            BowlingPlayer p = new BowlingPlayer("Leandro");

            // When

            //Then
            Assert.AreEqual(10, p.Turns.Length);
        }
        
    }
}
