using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BowlingGamePresenterBuilderShould
    {

        [Test]
        public void CreateApresenterWithAscoreboard()
        {            
            IInGameView view = new InGameViewTestDoubleDummy();
            BowlingGamePresenter bowlingGamePresenter = BowlingGamePresenterBuilder.Build(view);            
            Assert.AreNotEqual(bowlingGamePresenter.Scoreboard, null);
        }

        [Test]
        public void CreateApresenterWithAview()
        {
            IInGameView view = new InGameViewTestDoubleDummy();
            BowlingGamePresenter bowlingGamePresenter = BowlingGamePresenterBuilder.Build(view);
            Assert.AreNotEqual(bowlingGamePresenter.GameView, null);
        }
    }
}
