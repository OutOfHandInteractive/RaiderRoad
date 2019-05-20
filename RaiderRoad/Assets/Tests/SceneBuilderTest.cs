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
    public abstract class SceneBuilderTest
    {
        #region Static Prefab stuff
        protected static GameObject thiefPrefab, hooliganPrefab, bruiserPrefab;
        protected static GameObject factoryPrefab, RVprefab;
        protected static GameObject dadPrefab, momPrefab, sonPrefab, daughterPrefab;
        private static bool prefabsLoaded = false;

        protected static void LoadPrefabs()
        {
            if (prefabsLoaded)
            {
                return;
            }

            factoryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Vehicles/VehicleFactory.prefab");
            PurgeMeshColliders(factoryPrefab.GetComponent<VehicleFactoryManager>());
            RVprefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Vehicles/RV2.0 1.prefab");
            thiefPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/EnemyL.prefab");
            hooliganPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/EnemyM.prefab");
            bruiserPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/EnemyH.prefab");

            dadPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/PlayerDad.prefab");
            momPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/PlayerMom.prefab");
            sonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/PlayerSon.prefab");
            daughterPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/PlayerDaughter.prefab");

            prefabsLoaded = true;
        }

        private static void PurgeMeshColliders(VehicleFactoryManager manager)
        {
            PurgeMeshColliders(manager.lVehicleFactory, "Light/");
            PurgeMeshColliders(manager.mVehicleFactory, "Medium/");
            PurgeMeshColliders(manager.hVehicleFactory, "Heavy/");
        }

        private static void PurgeMeshColliders(VehicleFactory_I factory, string prefix)
        {
            PurgeMeshColliders(factory.Attachment, prefix + "Attachment/");
            PurgeMeshColliders(factory.Cab, prefix + "Cab/");
            PurgeMeshColliders(factory.Cargo, prefix + "Cargo/");
            PurgeMeshColliders(factory.Wheel, prefix + "Wheel/");
            PurgeMeshColliders(factory.Payload, prefix + "Payload/");
        }

        private static void PurgeMeshColliders(List<GameObject> objects, string prefix)
        {
            foreach (GameObject obj in objects)
            {
                foreach (MeshCollider collider in obj.GetComponentsInChildren<MeshCollider>())
                {
                    collider.enabled = false;
                    Debug.LogError("MeshCollider on object: " + prefix + Util.FullObjectPath(collider.gameObject));
                }
            }
        }
        #endregion
        
        public GameObject RV;

        private GameObject root;

        protected bool buildSetupVehicle = true;
        protected readonly VehicleFactoryManager.vehicleTypes vehicleType;
        protected GameObject factoryManager;
        protected GameObject vehicle;
        protected List<GameObject> spawnNodes;

        public SceneBuilderTest(VehicleFactoryManager.vehicleTypes type)
        {
            vehicleType = type;
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
            factoryManager = Object.Instantiate(factoryPrefab, root.transform);
            factoryManager.GetComponent<VehicleFactoryManager>().RV = RV;

            yield return new WaitForFixedUpdate();

            if (buildSetupVehicle)
            {
                BuildVehicle();
            }
        }

        protected GameObject BuildVehicle()
        {
            VehicleFactoryManager factoryMan = factoryManager.GetComponent<VehicleFactoryManager>();
            Assert.That(factoryMan, Is.Not.Null);
            vehicle = factoryMan.NewConstructVehicle(vehicleType, 0, Vector3.zero, 0);
            vehicle.transform.parent = root.transform;
            spawnNodes = new List<GameObject>();
            foreach (StatefulEnemyAI ai in vehicle.GetComponentsInChildren<StatefulEnemyAI>())
            {
                spawnNodes.Add(ai.transform.parent.gameObject);
                Object.Destroy(ai.gameObject);
            }
            Assert.That(spawnNodes, Is.Not.Empty);
            return vehicle;
        }

        protected GameObject Spawn(GameObject obj)
        {
            return Spawn(obj, root.transform);
        }

        protected GameObject Spawn(GameObject obj, Transform parent)
        {
            return Object.Instantiate(obj, parent);
        }

        [UnityTearDown]
        public IEnumerator DestroyScene()
        {
            Object.Destroy(root);
            yield return new WaitForFixedUpdate();
        }
    }
}
