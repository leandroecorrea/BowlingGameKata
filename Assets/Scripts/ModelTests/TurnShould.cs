using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TurnShould
    {
        private Turn turn;
        [SetUp]
        public void SetUp()
        {
            turn = new Turn();
        }
        [Test]
        public void HaveTenPins()
        {
            //Given

            //When

            //Then
            Assert.AreEqual(10, turn.Pins);
        }
        [Test]
        public void HaveTwoThrowsRemaining()
        {
            //Given

            //When

            //Then
            Assert.AreEqual(2,turn.ThrowsRemaining);
        }
        [Test]
        [TestCase(10, ExpectedResult = 10)]
        [TestCase(9, ExpectedResult = 9)]
        [TestCase(8, ExpectedResult = 8)]
        [TestCase(7, ExpectedResult = 7)]
        [TestCase(6, ExpectedResult = 6)]
        [TestCase(5, ExpectedResult = 5)]
        [TestCase(4, ExpectedResult = 4)]
        [TestCase(3, ExpectedResult = 3)]
        [TestCase(2, ExpectedResult = 2)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(0, ExpectedResult = 0)]
        public int ScoreAccordingToThrow(int thrownPins)
        {
            //Given

            //When
            turn.Throw(thrownPins);
            //Then
            return turn.Score();

        }
        [Test]
        public void NotEndWhenRemainingThrows()
        {
            //Given
            //When
            turn.Throw(4);
            //Then
            Assert.AreEqual(TurnStatusEnum.ONGOING, turn.Status);

        }
        [Test]
        public void EndNormalWhenLessThanTenPinsThrownInTwoShots()
        {
            turn.Throw(3);
            turn.Throw(4);
            Assert.AreEqual(TurnStatusEnum.NORMAL, turn.Status);
        }
        [Test]
        public void EndWithSpare()
        {
            //Given
            //When
            turn.Throw(6);
            turn.Throw(4);
            //Then
            Assert.AreEqual(TurnStatusEnum.SPARE, turn.Status);

        }
        [Test]
        public void EndWithStrike()
        {
            //Given
            //When
            turn.Throw(10);
            //Then
            Assert.AreEqual(TurnStatusEnum.STRIKE, turn.Status);
        }
        [Test]
        public void BeONGOING()
        {
            //Given
            //When
            turn.Throw(8);
            //Then
            Assert.AreEqual(TurnStatusEnum.ONGOING,turn.Status);
        }

       
    }
}
