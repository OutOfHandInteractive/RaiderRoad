using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Payload : MonoBehaviour {
	public enum payloadTypes { enemy, weapon }

	#region Variable Declarations
	// -------------------- public variables ---------------------
	public List<payloadTypes> payloadCode;

	// ------------------- nonpublic variables -------------------
	protected float wChance = 0f;
	#endregion

	#region Abstract Methods
	public abstract void Populate();
	protected abstract StatefulEnemyAI SelectEnemies();
    protected abstract Weapon SelectInteractable();
    protected abstract int GetSize();
	#endregion

	protected List<GameObject> Populate(GameObject[] nodes) {
        SetPayloadCode(wChance);
        Debug.Log("weapon chance: " + wChance);
        List<GameObject> payloadInstance = new List<GameObject>();

        for (int i = 0; i < GetSize(); i++) {
            Vector3 localPos = Vector3.zero;
            if (payloadCode[i] == payloadTypes.enemy) {
                payloadInstance.Add(Instantiate(SelectEnemies().gameObject,  nodes[i].transform));
                // HACK HACK HACK!!
                localPos = new Vector3(0, 0.42f, 0);
            }
            else if (payloadCode[i] == payloadTypes.weapon) {
                payloadInstance.Add(Instantiate(SelectInteractable().gameObject, nodes[i].transform));
                // HACK HACK HACK!!
                if(payloadInstance[i].GetComponentInChildren<cannon>() != null) {
                    localPos = new Vector3(0, 0.414f, 0);
                }
            }
            else {
                throw new System.Exception("Unknown payload code: " + payloadCode[i]);
            }
            payloadInstance[i].transform.localPosition = localPos;
            //payloadInstance[i].transform.position += payloadOffset(i);
        }
        return payloadInstance;
    }

    protected virtual Vector3 PayloadOffset(int i) {
        return new Vector3(0, 1f, 0);
    }

    protected void SetPayloadCode(float wChance) {
        //randomly select possible weapon position
        int decided = Random.Range(0, GetSize() - 1);
        
		//populate code
        for (int i = 0; i < GetSize(); i++) {
            if(i == decided) {   //if at position selected above
                if(Random.value < wChance) {         // if % chance of getting weapon reached
                    payloadCode[i] = payloadTypes.weapon;    //set position to weapon
                }
                else {   //else
                    payloadCode[i] = payloadTypes.enemy;    //set to enemy (default)
                }
            }
            else {               //else
                payloadCode[i] = payloadTypes.enemy;    //set to enemy (default)
            }
        }
    }

    public void SetWChance(float _WC) {
        wChance = _WC;
    }
}