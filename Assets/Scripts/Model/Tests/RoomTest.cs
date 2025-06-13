using NUnit.Framework;
using UnityEngine;
using Model;
using System.Collections.Generic;

namespace Model.Tests
{
    [TestFixture]
    public class RoomTest
    {
        [Test]
        public void DefaultConstructor_SetsDefaultsCorrectly()
        {
            Room room = new Room();

            Assert.IsFalse(room.isEntrance);
            Assert.IsFalse(room.isExit);
            Assert.IsFalse(room.hasPillar);
            Assert.IsNull(room.pillarType);
            Assert.AreEqual("5thScenes", room.sceneName);
            Assert.AreEqual(Vector2Int.zero, room.gridPosition);
            Assert.IsNotNull(room.usedExits);
            Assert.IsEmpty(room.usedExits);
        }

        [Test]
        public void Constructor_WithGridPosition_SetsPosition()
        {
            Vector2Int pos = new Vector2Int(2, 3);
            Room room = new Room(pos);

            Assert.AreEqual(pos, room.gridPosition);
            Assert.IsFalse(room.isEntrance);
            Assert.IsFalse(room.isExit);
            Assert.IsFalse(room.hasPillar);
            Assert.IsNull(room.pillarType);
            Assert.IsNull(room.sceneName);
            Assert.IsNotNull(room.usedExits);
            Assert.IsEmpty(room.usedExits);
        }

        [Test]
        public void Can_SetRoomFlags_AndPillarType()
        {
            Room room = new Room();
            room.isEntrance = true;
            room.isExit = true;
            room.hasPillar = true;
            room.pillarType = "A";

            Assert.IsTrue(room.isEntrance);
            Assert.IsTrue(room.isExit);
            Assert.IsTrue(room.hasPillar);
            Assert.AreEqual("A", room.pillarType);
        }

        [Test]
        public void UsedExits_CanAddAndRetrieve()
        {
            Room room = new Room();
            room.usedExits["North"] = new Vector2Int(1, 2);

            Assert.IsTrue(room.usedExits.ContainsKey("North"));
            Assert.AreEqual(new Vector2Int(1, 2), room.usedExits["North"]);
        }

        [Test]
        public void ToString_ContainsKeyDetails()
        {
            Room room = new Room(new Vector2Int(5, 7))
            {
                isEntrance = true,
                isExit = true,
                hasPillar = true,
                pillarType = "I",
                sceneName = "MyScene"
            };
            room.usedExits["East"] = new Vector2Int(6, 7);

            string output = room.ToString();

            StringAssert.Contains("Pos(5,7)", output);
            StringAssert.Contains("Entrance", output);
            StringAssert.Contains("Exit", output);
            StringAssert.Contains("Pillar(I)", output);
            StringAssert.Contains("Scene(MyScene)", output);
            StringAssert.Contains("East", output);
        }
    }
}
