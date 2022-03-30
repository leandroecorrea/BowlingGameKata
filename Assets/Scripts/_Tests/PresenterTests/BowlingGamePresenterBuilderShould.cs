using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BowlingGamePresenterBuilderShould
    {
        IInGameView _view;
        BowlingGamePresenter _presenter;
        [SetUp]
        public void SetUp()
        {
            _view = new InGameViewTestDoubleDummy();
            _presenter = BowlingGamePresenterBuilder.Build(_view);
        }
        [Test]
        public void CreateApresenterWithAscoreboard()
        {   
            Assert.AreNotEqual(_presenter.Scoreboard, null);
        }

        [Test]
        public void CreateApresenterWithAview()
        {            
            Assert.AreNotEqual(_presenter.GameView, null);
        }        
    }
}
