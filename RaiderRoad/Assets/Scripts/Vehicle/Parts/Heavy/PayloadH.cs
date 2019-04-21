using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadH : Payload {
	private const int PAYLOAD_SIZE = 6;

	public GameObject enemyNode01, enemyNode02, enemyNode03, enemyNode04, enemyNode05, enemyNode06;
	public List<StatefulEnemyAI> enemies;
    public List<Weapon> weapons;

    private List<GameObject> payloadInstance = new List<GameObject>();
	private System.Random rand = new System.Random();

	public override void Populate() {
        GameObject[] nodes = { enemyNode01, enemyNode02, enemyNode03, enemyNode04, enemyNode05, enemyNode06 };
        Populate(nodes);
	}

    protected override int GetSize()
    {
        return PAYLOAD_SIZE;
    }

    protected override StatefulEnemyAI SelectEnemies() {
		int selectedIndex = rand.Next(0, enemies.Count);
		return enemies[selectedIndex];
	}
    protected override Weapon SelectInteractable()
    {
        int selectedIndex = rand.Next(0, weapons.Count);
        return weapons[selectedIndex];
    }

}
