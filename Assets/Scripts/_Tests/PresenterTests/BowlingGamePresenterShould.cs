using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BowlingGamePresenterShould
    {
        
        [Test]
        public void HaveTwoPlayers()
        {
            BowlingGamePresenter gamePresenter = new BowlingGamePresenter();
            Assert.AreEqual(2, 0);
        }

    }
}
