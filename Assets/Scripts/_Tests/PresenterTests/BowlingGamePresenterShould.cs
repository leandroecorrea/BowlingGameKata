using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BowlingGamePresenterShould
    {
        BowlingGamePresenter _presenter;
        [SetUp]
        public void SetUp()
        {
            _presenter = BowlingGamePresenterBuilder.Build(new InGameViewTestDoubleDummy());
        }
        [Test]
        public void BePlayersOneTurn()
        {
            var result = _presenter.CurrentPlayer;
            Assert.AreEqual(_presenter.Scoreboard.Players[0], result);
        }

        [Test]
        public void BePlayersTwoTurnAfterTwoThrows()
        {
            _presenter.ReceiveThrow(4);
            _presenter.ReceiveThrow(5);
            var result = _presenter.CurrentPlayer;
            Assert.AreEqual(_presenter.Scoreboard.Players[1], result);
        }

        [Test]
        public void BePlayersTwoTurnAfterAstrike()
        {
            _presenter.ReceiveThrow(10);
            var result = _presenter.CurrentPlayer;
            Assert.AreEqual(_presenter.Scoreboard.Players[1], result);
        }

        [Test]
        public void BePlayersOneTurnAgain()
        {
            _presenter.ReceiveThrow(0);
            _presenter.ReceiveThrow(0);
            _presenter.ReceiveThrow(0);
            _presenter.ReceiveThrow(0);
            var result = _presenter.CurrentPlayer;
            Assert.AreEqual(_presenter.Scoreboard.Players[0], result);
        }
    }
}
