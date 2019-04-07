using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadM : Payload {
	private const int PAYLOAD_SIZE = 4;

	#region Variable Declarations
	// -------------------- public variables -----------------------
	public GameObject enemyNode01, enemyNode02, enemyNode03, enemyNode04;
	public List<StatefulEnemyAI> enemies;
    public List<Weapon> weapons;

	// ------------------- nonpublic variables ---------------------
    private List<GameObject> payloadInstance = new List<GameObject>();
	private System.Random rand = new System.Random();
	#endregion

	public override void Populate() {
        GameObject[] nodes = { enemyNode01, enemyNode02, enemyNode03, enemyNode04 };
        Populate(nodes);
    }

    protected override Vector3 PayloadOffset(int i) {
        switch (i) {
            case 1: return new Vector3(1f, 1f, 0);
            case 2: return new Vector3(-1f, 1f, 0);
            case 3: return new Vector3(0, 1f, -1f);
        }
        return base.PayloadOffset(i);
    }

    protected override int GetSize() {
        return PAYLOAD_SIZE;
    }

    protected override StatefulEnemyAI SelectEnemies() {
		int selectedIndex = rand.Next(0, enemies.Count); // error getting thrown here, null ref exception
		return enemies[selectedIndex];
	}

    protected override Weapon SelectInteractable()
    {
        int selectedIndex = rand.Next(0, weapons.Count);
        return weapons[selectedIndex];
    }
}
