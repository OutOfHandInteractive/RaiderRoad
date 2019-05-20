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
    public class VehicleFactoryTest : SceneBuilderTest
    {
        public VehicleFactoryTest(VehicleFactoryManager.vehicleTypes type) : base(type)
        {
            buildSetupVehicle = false;
        }

        // A Test behaves as an ordinary method
        [Test]
        public void VehicleFactoryTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        private IEnumerable<GameObject> AllPermutations()
        {
            var factory = GetFactory();
            List<GameObject> Cabs = new List<GameObject>(factory.Cab);
            List<GameObject> Cargoes = new List<GameObject>(factory.Cargo);
            List<GameObject> Attachments = new List<GameObject>(factory.Attachment);
            List<GameObject> Wheels = new List<GameObject>(factory.Wheel);
            List<GameObject> Payloads = new List<GameObject>(factory.Payload);
            foreach (GameObject cab in Cabs)
            {
                factory.Cab = new List<GameObject> { cab };
                foreach (GameObject cargo in Cargoes)
                {
                    factory.Cargo = new List<GameObject> { cargo };
                    foreach (GameObject attachment in Attachments)
                    {
                        factory.Attachment = new List<GameObject> { attachment };
                        foreach (GameObject wheel in Wheels)
                        {
                            factory.Wheel = new List<GameObject> { wheel };
                            foreach (GameObject payload in Payloads)
                            {
                                factory.Payload = new List<GameObject> { payload };
                                var res = BuildVehicle();
                                res.name = cab.name + ':' + cargo.name + ':' + attachment.name + ':' + wheel.name + ':' + payload.name;
                                yield return res;
                            }
                        }
                    }
                }
            }
            factory.Cab = Cabs;
            factory.Cargo = Cargoes;
            factory.Attachment = Attachments;
            factory.Wheel = Wheels;
            factory.Payload = Payloads;
        }
        
        [UnityTest]
        public IEnumerator VehicleFactoryTest_AllPermutations()
        {
            foreach(GameObject vehicle in AllPermutations())
            {
                Assert.That(Util.IsVehicle(vehicle), Is.True);
                var chassis = vehicle.GetComponentInChildren<Chassis>();
                Assert.That(chassis, Is.Not.Null);
                Assert.That(vehicle.GetComponentInChildren<Cab>(), Is.Not.Null);
                Assert.That(vehicle.GetComponentInChildren<Attachment>(), Is.Not.Null);
                Assert.That(vehicle.GetComponentInChildren<Payload>(), Is.Not.Null);
                Assert.That(vehicle.GetComponentInChildren<Wheel>(), Is.Not.Null);
                
                float armorStacks = 0f;
                float ramDamageStacks = 0f;
                float speedStacks = 0f;
                int threatMod = 0;
                foreach(DestructiblePart part in vehicle.GetComponentsInChildren<DestructiblePart>())
                {
                    armorStacks += part.armorStacks;
                    ramDamageStacks += part.ramDamageStacks;
                    speedStacks += part.speedStacks;
                    threatMod += part.threatModifier;
                }

                var vAI = vehicle.GetComponent<VehicleAI>();
                Assert.That(vAI, Is.Not.Null);
                Assert.That(vAI.getMaxHealth(), Is.EqualTo(chassis.GetBaseHealth() * (1 + armorStacks * Constants.ARMOR_TOTALHEALTH_MODIFIER_PER_STACK)));
                Assert.That(vAI.getRamDamage(), Is.EqualTo(ramDamageStacks));
                Assert.That(vAI.getSpeed(), Is.EqualTo(chassis.baseSpeed * (1 + speedStacks * Constants.SPEED_MOVEMENT_MODIFIER_PER_STACK)),
                    "Speed is incorrect: {0}, baseSpeed: {1}, speedStacks: {2}", vehicle.name, chassis.baseSpeed, speedStacks);
                Assert.That(vAI.getMovementChance(), Is.EqualTo(speedStacks * Constants.SPEED_LOCATIONCHANGE_MODIFIER_PER_STACK));

                var eve = vehicle.GetComponent<eventObject>();
                Assert.That(eve, Is.Not.Null);
                Assert.That(eve.getDifficulty(), Is.EqualTo(chassis.baseThreat + threatMod));
                // TODO: More tests

                // Cleanup
                Object.Destroy(vehicle);
                yield return new WaitForFixedUpdate();
            }
        }

        private VehicleFactory_I GetFactory()
        {
            switch (vehicleType)
            {
                case VehicleFactoryManager.vehicleTypes.heavy: return factoryManager.GetComponentInChildren<VehicleFactoryH>();
                case VehicleFactoryManager.vehicleTypes.medium: return factoryManager.GetComponentInChildren<VehicleFactoryM>();
                case VehicleFactoryManager.vehicleTypes.light: return factoryManager.GetComponentInChildren<VehicleFactoryL>();
                default: Debug.LogError("Unknown vehicle type: "+vehicleType); return null;
            }
        }
    }
}
