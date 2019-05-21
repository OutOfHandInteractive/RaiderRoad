using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture(VehicleFactoryManager.vehicleTypes.heavy)]
    [TestFixture(VehicleFactoryManager.vehicleTypes.medium)]
    [TestFixture(VehicleFactoryManager.vehicleTypes.light)]
    public class VehicleAITest : SceneBuilderTest
    {
        public VehicleAITest(VehicleFactoryManager.vehicleTypes type) : base(type)
        {
        }
        
        [UnityTest]
        public IEnumerator VehicleAITest_VehiclesDie()
        {
            var vAI = vehicle.GetComponent<VehicleAI>();
            Assert.That(vAI, Is.Not.Null, "Vehicle AI should exist");
            vAI.takeDamage(vAI.getMaxHealth());
            Assert.That(vehicle == null, Is.False, "Vehicle should not die right away");
            yield return new WaitForSeconds(5.1f);
            Assert.That(vehicle == null, Is.True, "Vehicle should be dead by now");
        }
    }
}
