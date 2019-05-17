using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using Cinemachine;
using System;
using Object = UnityEngine.Object;

namespace Tests
{
    [TestFixture(VehicleFactoryManager.vehicleTypes.heavy)]
    [TestFixture(VehicleFactoryManager.vehicleTypes.medium)]
    [TestFixture(VehicleFactoryManager.vehicleTypes.light)]
    public class EnemyAITests : SceneBuilderTest
    {
        public EnemyAITests(VehicleFactoryManager.vehicleTypes type) : base(type)
        {
        }

        [UnityTest]
        public IEnumerator EnemyAITests_StartInWaitState()
        {
            // Use the Assert class to test conditions
            GameObject enemy = Object.Instantiate(thiefPrefab, spawnNodes[0].transform);
            StatefulEnemyAI ai = enemy.GetComponent<StatefulEnemyAI>();
            Assert.That(ai, Is.Not.Null);
            Assert.That(ai.GetState(), Is.EqualTo(StatefulEnemyAI.State.Wait));
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator EnemyAITests_Control()
        {
            yield return null;
        }
    }
}
