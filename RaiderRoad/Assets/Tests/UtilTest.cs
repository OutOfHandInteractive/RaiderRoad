using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace Tests
{
    [TestFixture(VehicleFactoryManager.vehicleTypes.heavy)]
    [TestFixture(VehicleFactoryManager.vehicleTypes.medium)]
    [TestFixture(VehicleFactoryManager.vehicleTypes.light)]
    public class UtilTest : SceneBuilderTest
    {
        public UtilTest(VehicleFactoryManager.vehicleTypes type) : base(type)
        {
        }
        
        [Test]
        public void UtilTest_IsVehicle()
        {
            Assert.That(Util.IsVehicle(vehicle), Is.True);
            Assert.That(Util.IsVehicleRecursive(vehicle), Is.True);

            var thief = Spawn(thiefPrefab, spawnNodes[0].transform);
            Assert.That(Util.IsVehicle(thief), Is.False);
            Assert.That(Util.IsVehicleRecursive(thief), Is.True);

            Assert.That(Util.IsVehicle(thiefPrefab), Is.False);
            Assert.That(Util.IsVehicle(hooliganPrefab), Is.False);
            Assert.That(Util.IsVehicle(bruiserPrefab), Is.False);
            Assert.That(Util.IsVehicle(RV), Is.False);
            Assert.That(Util.IsVehicle(dadPrefab), Is.False);
            Assert.That(Util.IsVehicle(momPrefab), Is.False);
            Assert.That(Util.IsVehicle(sonPrefab), Is.False);
            Assert.That(Util.IsVehicle(daughterPrefab), Is.False);
        }

        [Test]
        public void UtilTest_IsPlayer()
        {
            CheckPlayer(dadPrefab);
            CheckPlayer(Spawn(dadPrefab));

            CheckPlayer(momPrefab);
            CheckPlayer(Spawn(momPrefab));

            CheckPlayer(sonPrefab);
            CheckPlayer(Spawn(sonPrefab));

            CheckPlayer(daughterPrefab);
            CheckPlayer(Spawn(daughterPrefab));

            Assert.That(Util.isPlayer(vehicle), Is.False);
            Assert.That(Util.isPlayer(thiefPrefab), Is.False);
            Assert.That(Util.isPlayer(hooliganPrefab), Is.False);
            Assert.That(Util.isPlayer(bruiserPrefab), Is.False);
            Assert.That(Util.isPlayer(RV), Is.False);
        }

        private static void CheckPlayer(GameObject player)
        {
            Assert.That(Util.isPlayer(player), Is.True);
        }
    }
}
