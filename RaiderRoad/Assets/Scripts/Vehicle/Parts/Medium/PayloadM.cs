using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadM : Payload {
	private const int PAYLOAD_SIZE = 4;

	public GameObject enemyNode01, enemyNode02, enemyNode03, enemyNode04;
	public List<EnemyAI> enemies;

	private List<GameObject> payloadInstance;

	private void Start() {
		payloadInstance = new List<GameObject>();
		rand = new System.Random();
	}

	public override void populate() {
		for (int i = 0; i < PAYLOAD_SIZE; i++) {
			if (payloadCode[i] == payloadTypes.enemy)
				payloadInstance.Add(Instantiate(SelectEnemies().gameObject));
		}

		payloadInstance[0].transform.SetParent(enemyNode01.transform);
		payloadInstance[0].transform.position = Vector3.zero;

		payloadInstance[1].transform.SetParent(enemyNode02.transform);
		payloadInstance[1].transform.position = Vector3.zero;

		payloadInstance[2].transform.SetParent(enemyNode03.transform);
		payloadInstance[2].transform.position = Vector3.zero;

		payloadInstance[3].transform.SetParent(enemyNode04.transform);
		payloadInstance[3].transform.position = Vector3.zero;
	}

	protected override EnemyAI SelectEnemies() {
		Debug.Log("enemy prefab count " + enemies.Count);
		int selectedIndex = rand.Next(0, enemies.Count); // error getting thrown here, null ref exception
		return enemies[selectedIndex];
	}
}
