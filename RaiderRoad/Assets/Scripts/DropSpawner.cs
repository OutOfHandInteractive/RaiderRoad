using UnityEngine;
using System.Collections.Generic;

public class DropSpawner : MonoBehaviour
{
    private static List<DropSpawner> spawners = new List<DropSpawner>();

    public GameObject springTrap;
    public GameObject lureTrap;

    public static DropSpawner GetSpawn()
    {
        return spawners[0];
    }

    // Use this for initialization
    void Start()
    {
        spawners.Add(this);
    }

    public GameObject SpawnSpringTrap()
    {
        return Spawn(springTrap);
    }

    public GameObject SpawnLureTrap()
    {
        return Spawn(lureTrap);
    }
    
    public GameObject Spawn(GameObject drop)
    {
        return Instantiate(drop, Util.Copy(this.transform.position), Quaternion.identity, this.transform.parent);
    }
}
