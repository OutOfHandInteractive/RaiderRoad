using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventObject : MonoBehaviour {

	private int difficulty;
	
	public void setDifficulty(int _diff) {
		difficulty = _diff;
	}

	public int getDifficulty() {
		return difficulty;
	}
}
