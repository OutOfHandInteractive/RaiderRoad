using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Payload : MonoBehaviour {
	public enum payloadTypes { enemy, weapon, battery }

	#region Variable Declarations
	// -------------------- public variables ---------------------
	public List<payloadTypes> payloadCode;
    public GameObject batteryPickup;

	// ------------------- nonpublic variables -------------------
	protected float wChance = 0f;
    protected int batteriesLeft = 0;
	#endregion

	#region Abstract Methods
	public abstract void Populate();
	protected abstract StatefulEnemyAI SelectEnemies();
    protected abstract Weapon SelectInteractable();
    protected abstract int GetSize();
	#endregion

	protected List<GameObject> Populate(GameObject[] nodes) {
        SetPayloadCode(wChance, getBatChance(batteriesLeft));
        //Debug.Log("weapon chance: " + wChance);
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
            }else if(payloadCode[i] == payloadTypes.battery){
                payloadInstance.Add(Instantiate(batteryPickup, nodes[i].transform));
                // HACK HACK HACK!!
                localPos = new Vector3(0, 0.42f, 0);
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

    protected void SetPayloadCode(float wChance, float batChance) {
        //randomly select possible weapon position
        int decided = Random.Range(0, GetSize() - 1);
        
		//populate code
        for (int i = 0; i < GetSize(); i++) {
            if(i == decided) {   //if at position selected above
                if(batChance > 0f && Random.value < batChance){ //need a battery & chance met
                    payloadCode[i] = payloadTypes.battery;    //set position to battery
                }
                else if(Random.value < wChance) {         // if % chance of getting weapon reached
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
    public void SetBatteriesLeft(int num)
    {
        batteriesLeft = num;
    }
    public float getBatChance(int batteries)
    {
        if(batteries >= 3){         //all batteries present
            return 0f;                 //no batteries spawn as payload
        }else if(batteries == 2){   //missing one battery
            return 0.15f;    //minor chance of a battery
        }else if(batteries == 1){   //missing two batteries
            return 0.5f;    //50/50 shot at getting a battery
        }else{                      //missing all the batteries - game over imminent
            return 1f;  //guarantee a battery
        }
    }
}