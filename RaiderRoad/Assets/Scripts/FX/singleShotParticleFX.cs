using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleShotParticleFX : MonoBehaviour {

	public ParticleSystem fx;
	
	void Update () {
		if (!fx.IsAlive())
			Destroy(gameObject);
	}
}