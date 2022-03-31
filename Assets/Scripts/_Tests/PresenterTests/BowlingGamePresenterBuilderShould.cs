using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;

namespace Tests
{
    public class BowlingGamePresenterBuilderShould
    {
        BowlingGamePresenter _presenter;
        Mock<IInGameView> _view;

        [SetUp]
        public void SetUp()
        {
            _view = new Mock<IInGameView>();
            _presenter = BowlingGamePresenterBuilder.Build(_view.Object);
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
