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

        private GameObject[] Prefabs()
        {
            return new GameObject[] { thiefPrefab, hooliganPrefab, bruiserPrefab };
        }

        [UnityTest]
        public IEnumerator EnemyAITests_StartInWaitState()
        {
            foreach(GameObject prefab in Prefabs())
            {
                // Use the Assert class to test conditions
                GameObject enemy = Object.Instantiate(prefab, spawnNodes[0].transform);
                StatefulEnemyAI ai = enemy.GetComponent<StatefulEnemyAI>();
                Assert.That(ai, Is.Not.Null);
                Assert.That(ai.GetState(), Is.EqualTo(StatefulEnemyAI.State.Wait));
                Object.Destroy(enemy);
                yield return new WaitForFixedUpdate();
            }
        }
        
        [UnityTest]
        public IEnumerator EnemyAITests_Control()
        {
            yield return null;
        }
    }
}
