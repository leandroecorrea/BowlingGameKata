using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ScoreboardShould
    {
        Scoreboard scoreboard;
        BowlingPlayer playerOne;
        BowlingPlayer playerTwo;
        [SetUp]
        public void SetUp()
        {
            playerOne = new BowlingPlayer("German");
            playerTwo = new BowlingPlayer("Leandro");
            IBowlingPlayer[] players = new IBowlingPlayer[] { playerOne, playerTwo };
            scoreboard = new Scoreboard(players);
        }
        [Test]
        public void ScoreboardShouldHaveTwoPlayers()
        {
            Assert.AreEqual(2, scoreboard.Players.Count);
        }

        [Test]
        [TestCase(8, 0, ExpectedResult = 8)]
        [TestCase(2, 5, ExpectedResult = 7)]
        [TestCase(0, 0, ExpectedResult = 0)]
        [TestCase(5, 4, ExpectedResult = 9)]
        public int ScoreNormal(int firstThrowPinsAmount, int secondThrowPinsAmount)
        {
            // Given
            IBowlingPlayer bowlingPlayer = scoreboard.Players[0];

            // When
            bowlingPlayer.Throw(firstThrowPinsAmount);
            bowlingPlayer.Throw(secondThrowPinsAmount);

            //Then
            var result = bowlingPlayer.Turns[0].Status;
            Assert.AreEqual(TurnStatusEnum.NORMAL, result);
            return scoreboard.ScoreForTurn(playerOne, 0);
        }

        [Test]
        public void ScoreWhenSpare()
        {
            // Given
            IBowlingPlayer bowlingPlayer = scoreboard.Players[0];

            //When
            bowlingPlayer.Throw(0);
            bowlingPlayer.Throw(10);
            bowlingPlayer.Throw(4);
            bowlingPlayer.Throw(3);

            // Then
            var result = bowlingPlayer.Turns[0].Status;
            Assert.AreEqual(TurnStatusEnum.SPARE, result);
            Assert.AreEqual(14, scoreboard.ScoreForTurn(bowlingPlayer, 0));
        }

        [Test]
        public void ScoreWhenStrike()
        {
            // Given
            IBowlingPlayer bowlingPlayer = scoreboard.Players[0];

            //When
            bowlingPlayer.Throw(10);
            bowlingPlayer.Throw(4);
            bowlingPlayer.Throw(3);

            // Then
            var result = bowlingPlayer.Turns[0].Status;
            Assert.AreEqual(TurnStatusEnum.STRIKE, result);
            Assert.AreEqual(17, scoreboard.ScoreForTurn(bowlingPlayer, 0));
        }
        [Test]
        public void GiveExtraThrowOnSpareLastTurn()
        {
            // Given
            IBowlingPlayer bowlingPlayer = scoreboard.Players[0];

            //When
            NineDummyShotsFor(bowlingPlayer);
            bowlingPlayer.Throw(8);
            bowlingPlayer.Throw(2);
            bowlingPlayer.Throw(2);

            // Then
            var result = bowlingPlayer.Turns[Turn.LAST_TURN_INDEX].Status;
            Assert.AreEqual(TurnStatusEnum.SPARE, result);
            Assert.AreEqual(12, scoreboard.ScoreForTurn(bowlingPlayer, 9));
        }
        [Test]
        public void GiveTwoExtraThrowOnStrikeLastTurn()
        {
            // Given
            IBowlingPlayer bowlingPlayer = scoreboard.Players[0];

            //When
            NineDummyShotsFor(bowlingPlayer);
            bowlingPlayer.Throw(10);
            bowlingPlayer.Throw(5);
            bowlingPlayer.Throw(5);


            // Then
            var result = bowlingPlayer.Turns[Turn.LAST_TURN_INDEX].Status;
            Assert.AreEqual(TurnStatusEnum.STRIKE, result);
            Assert.AreEqual(20, scoreboard.ScoreForTurn(bowlingPlayer, 9));
        }
        private void NineDummyShotsFor(IBowlingPlayer player)
        {
            for(int i = 0; i < 9; i++)
            {
                player.Throw(10);
            }
        }
        
    }
}
