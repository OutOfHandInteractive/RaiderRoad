using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackEnemy : EnemyAI {

    private GameObject cMunnitions;
    private GameObject proj;
    private GameObject cObject;
    private VehicleAI eVehicle;
    private GameObject cannon;
    private GameObject barrel;
    public void StartWeapon(GameObject enemy, VehicleAI vehicle, GameObject munnitions)
    {
        cObject = enemy;
        eVehicle = vehicle;
        cMunnitions = munnitions;
        cannon = GameObject.Find("Cannon");
        barrel = GameObject.Find("Barrel");
    }
	
	// Update is called once per frame
	public void Weapon () {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);
        cannon.transform.LookAt(player.transform.position);
        proj = Instantiate(cMunnitions.gameObject, barrel.transform.position, Quaternion.identity);
        proj.GetComponent<cannonball>().launch(player.transform.position, barrel.transform.position);

    }
}
