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

    protected List<GameObject> populate(GameObject[] nodes)
    {
        List<GameObject> payloadInstance = new List<GameObject>();
        for (int i = 0; i < GetSize(); i++)
        {
            Vector3 localPos = Vector3.zero;
            if (payloadCode[i] == payloadTypes.enemy)
            {
                payloadInstance.Add(Instantiate(SelectEnemies().gameObject,  nodes[i].transform));
                // HACK HACK HACK!!
                localPos = new Vector3(0, 0.42f, 0);
            }
            else if (payloadCode[i] == payloadTypes.weapon)
            {
                payloadInstance.Add(Instantiate(SelectInteractable().gameObject, nodes[i].transform));
                // HACK HACK HACK!!
                if(payloadInstance[i].GetComponentInChildren<cannon>() != null)
                {
                    localPos = new Vector3(0, 0.414f, 0);
                }
            }
            else
            {
                throw new System.Exception("Unknown payload code: " + payloadCode[i]);
            }
            payloadInstance[i].transform.localPosition = localPos;
            //payloadInstance[i].transform.position += payloadOffset(i);
        }
        return payloadInstance;
    }

    protected virtual Vector3 payloadOffset(int i)
    {
        return new Vector3(0, 1f, 0);
    }
}