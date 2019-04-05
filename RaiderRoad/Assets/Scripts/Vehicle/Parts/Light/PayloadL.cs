using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadL : Payload {
	private const int PAYLOAD_SIZE = 2;

	#region Variable Declarations
	// --------------------- public variables ---------------------
	public GameObject enemyNode01, enemyNode02;
	public List<StatefulEnemyAI> enemies;
    public List<Weapon> weapons;

	// -------------------- nonpublic variables -------------------
	private List<GameObject> payloadInstance = new List<GameObject>();
	private System.Random rand = new System.Random();
	#endregion

	public override void Populate() {
        GameObject[] nodes = { enemyNode01, enemyNode02 };
        Populate(nodes);
    }

    protected override int GetSize() {
        return PAYLOAD_SIZE;
    }

    protected override StatefulEnemyAI SelectEnemies() {
		int selectedIndex = rand.Next(0, enemies.Count);
		return enemies[selectedIndex];
	}

    protected override Weapon SelectInteractable() {
        int selectedIndex = rand.Next(0, weapons.Count);
        return weapons[selectedIndex];
    }
}
