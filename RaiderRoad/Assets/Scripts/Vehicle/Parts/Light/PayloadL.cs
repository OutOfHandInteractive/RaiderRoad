using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadL : Payload {
	private const int PAYLOAD_SIZE = 2;

	public GameObject enemyNode01, enemyNode02;
	public List<EnemyAI> enemies;

	private List<GameObject> payloadInstance = new List<GameObject>();
	private System.Random rand = new System.Random();

	public override void populate() {
		for (int i=0; i < PAYLOAD_SIZE; i++) {
			if (payloadCode[i] == payloadTypes.enemy)
				payloadInstance.Add(Instantiate(SelectEnemies().gameObject));
		}

		payloadInstance[0].transform.SetParent(enemyNode01.transform);
		payloadInstance[0].transform.position = Vector3.zero;

		payloadInstance[1].transform.SetParent(enemyNode02.transform);
		payloadInstance[1].transform.position = Vector3.zero;
	}

	protected override EnemyAI SelectEnemies() {
		int selectedIndex = rand.Next(0, enemies.Count);
		return enemies[selectedIndex];
	}
}
