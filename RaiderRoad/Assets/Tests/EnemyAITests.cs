using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using Cinemachine;

namespace Tests
{
    [TestFixture(VehicleFactoryManager.vehicleTypes.heavy)]
    [TestFixture(VehicleFactoryManager.vehicleTypes.medium)]
    [TestFixture(VehicleFactoryManager.vehicleTypes.light)]
    public class EnemyAITests
    {
        private GameObject thiefPrefab, hooliganPrefab, bruiserPrefab;
        private GameObject factoryPrefab, RVprefab;
        private bool prefabsLoaded = false;

        public GameObject player;
        public GameObject RV;

        private GameObject root;

        private readonly VehicleFactoryManager.vehicleTypes vehicleType;
        private GameObject vehicle;
        private List<GameObject> spawnNodes;

        public EnemyAITests(VehicleFactoryManager.vehicleTypes type)
        {
            vehicleType = type;
        }

        private void LoadPrefabs()
        {
            if (prefabsLoaded)
            {
                return;
            }

            factoryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Vehicles/VehicleFactory.prefab");
            RVprefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Vehicles/RV2.0 1.prefab");
            thiefPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/EnemyL.prefab");
            hooliganPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/EnemyM.prefab");
            bruiserPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/EnemyH.prefab");

            prefabsLoaded = true;
        }

        [UnitySetUp]
        public IEnumerator BuildScene()
        {
            LoadPrefabs();

            root = new GameObject("ROOT");
            var cam = new GameObject("MainVCam");
            cam.transform.parent = root.transform;
            cam.AddComponent<CameraShake>();
            cam.AddComponent<CinemachineVirtualCamera>().AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cam.tag = "MainVCam";
            
            root = Object.Instantiate(root);

            RV = Object.Instantiate(RVprefab, new Vector3(100, 0, 0), Quaternion.identity, root.transform);
            Assert.That(RV.GetComponent<rvHealth>(), Is.Not.Null);
            var factoryManager = Object.Instantiate(factoryPrefab, root.transform);
            factoryManager.GetComponent<VehicleFactoryManager>().RV = RV;

            yield return new WaitForFixedUpdate();

            VehicleFactoryManager factoryMan = factoryManager.GetComponent<VehicleFactoryManager>();
            Assert.That(factoryMan, Is.Not.Null);
            vehicle = factoryMan.NewConstructVehicle(vehicleType, 0, Vector3.zero, 0);
            vehicle.transform.parent = root.transform;
            spawnNodes = new List<GameObject>();
            foreach(StatefulEnemyAI ai in vehicle.GetComponentsInChildren<StatefulEnemyAI>()){
                spawnNodes.Add(ai.transform.parent.gameObject);
                Object.Destroy(ai.gameObject);
            }
            Assert.That(spawnNodes, Is.Not.Empty);
        }

        [UnityTearDown]
        public IEnumerator DestroyScene()
        {
            Object.Destroy(root);
            yield return new WaitForFixedUpdate();
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
