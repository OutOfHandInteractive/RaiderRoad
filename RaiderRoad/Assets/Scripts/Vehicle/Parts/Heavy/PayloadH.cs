using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadH : Payload {
	private const int PAYLOAD_SIZE = 6;

	public GameObject enemyNode01, enemyNode02, enemyNode03, enemyNode04, enemyNode05, enemyNode06;
	public List<StatefulEnemyAI> enemies;
    public List<Interactable> weapons;

    private List<GameObject> payloadInstance = new List<GameObject>();
	private System.Random rand = new System.Random();

	public override void populate() {
		for (int i = 0; i < PAYLOAD_SIZE; i++) {
			if (payloadCode[i] == payloadTypes.enemy)
				payloadInstance.Add(Instantiate(SelectEnemies().GetEnemyObject()));
            if (payloadCode[i] == payloadTypes.weapon)
            {
                payloadInstance.Add(Instantiate(SelectInteractable().gameObject));
            }
        }

		payloadInstance[0].transform.SetParent(enemyNode01.transform);
		payloadInstance[0].transform.position = new Vector3(0, 1f, 0);

		payloadInstance[1].transform.SetParent(enemyNode02.transform);
		payloadInstance[1].transform.position = new Vector3(0, 1f, 0);

		payloadInstance[2].transform.SetParent(enemyNode03.transform);
		payloadInstance[2].transform.position = new Vector3(0, 1f, 0);

		payloadInstance[3].transform.SetParent(enemyNode04.transform);
		payloadInstance[3].transform.position = new Vector3(0, 1f, 0);

		payloadInstance[4].transform.SetParent(enemyNode05.transform);
		payloadInstance[4].transform.position = new Vector3(0, 1f, 0);

		payloadInstance[5].transform.SetParent(enemyNode06.transform);
		payloadInstance[5].transform.position = new Vector3(0, 1f, 0);
	}

	protected override StatefulEnemyAI SelectEnemies() {
		int selectedIndex = rand.Next(0, enemies.Count);
		return enemies[selectedIndex];
	}
    protected override Interactable SelectInteractable()
    {
        int selectedIndex = rand.Next(0, weapons.Count);
        return weapons[selectedIndex];
    }
}
