using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class BowlingGamePresenterShould
    {
        BowlingGamePresenter _presenter;
        Mock<IInGameView> _gameView;
        [SetUp]
        public void SetUp()
        {
            _gameView = new Mock<IInGameView>();
            _gameView.Setup(v => v.GetPlayerTurnViewDetailFor(It.IsAny<int>(), It.IsAny<int>())).Returns(GetPlayerTurnViewDetailForMock());
            _presenter = BowlingGamePresenterBuilder.Build(_gameView.Object);
        }

        private TurnViewDetail GetPlayerTurnViewDetailForMock()
        {
            GameObject firstThrow = new GameObject();
            GameObject secondThrow = new GameObject();
            GameObject thirdThrow = new GameObject();
            GameObject finalScore = new GameObject();
            firstThrow.AddComponent<Text>();
            secondThrow.AddComponent<Text>();
            thirdThrow.AddComponent<Text>();
            finalScore.AddComponent<Text>();
            return new TurnViewDetail(firstThrow, secondThrow, thirdThrow, finalScore);
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

        [Test]
        public void UpdatePlayersNameAfterThrowWithSamePlayer()
        {
            //When
            _presenter.ReceiveThrow(5);

            //Then
            _gameView.Verify(v => v.UpdatePlayerTurnName("German"));
        }

        [Test]
        [TestCase(4, 4)]
        [TestCase(0, 9)]
        [TestCase(5, 5)]
        [TestCase(0, 10)]
        public void UpdatePlayersNameAfterTwoThrowsWithTheOtherPlayer(int firstThrow, int secondThrow)
        {
            //When
            _presenter.ReceiveThrow(firstThrow);
            _presenter.ReceiveThrow(secondThrow);
            //Then
            _gameView.Verify(v => v.UpdatePlayerTurnName("Lean"));
        }

        [Test]
        public void UpdatePlayersNameAfterAstrikeWithTheOtherPlayer()
        {
            //When
            _presenter.ReceiveThrow(10);
            //Then
            _gameView.Verify(v => v.UpdatePlayerTurnName("Lean"));
        }

        [Test]
        public void UpdatePlayersNameAfterTwoStrikesWithTheFirstPlayer()
        {
            //When
            _presenter.ReceiveThrow(10);
            _presenter.ReceiveThrow(10);
            //Then            
            _gameView.Verify(v => v.UpdatePlayerTurnName("German"));
        }
        [Test]
        public void UpdatePlayersNameAfterTwoTurnsWithFirstPlayer()
        {
            _presenter.ReceiveThrow(1);
            _presenter.ReceiveThrow(1);
            _presenter.ReceiveThrow(1);
            _presenter.ReceiveThrow(1);
            _presenter.ReceiveThrow(1);
            _gameView.Verify(v => v.UpdatePlayerTurnName("German"), Times.Exactly(3));
        }
    }
}
