using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Payload : MonoBehaviour {
	public enum payloadTypes { enemy, weapon }
	public abstract void populate();
	public List<payloadTypes> payloadCode;

	protected abstract StatefulEnemyAI SelectEnemies();
    protected abstract Weapon SelectInteractable();

    protected abstract int GetSize();

    protected void populate(GameObject[] nodes)
    {
        List<GameObject> payloadInstance = new List<GameObject>();
        for (int i = 0; i < GetSize(); i++)
        {
            if (payloadCode[i] == payloadTypes.enemy)
                payloadInstance.Add(Instantiate(SelectEnemies().gameObject, nodes[i].transform));
            if (payloadCode[i] == payloadTypes.weapon)
            {
                payloadInstance.Add(Instantiate(SelectInteractable().gameObject, nodes[i].transform));
            }
            payloadInstance[i].transform.position += new Vector3(0, 1f, 0);
        }
    }
}