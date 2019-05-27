using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscBreakable : MiscHittable
{
    [SerializeField] private ParticleSystem objectBreakParticles;
    [SerializeField] private int hitsToBreak = 3;
	[SerializeField] private GameObject locator;
	[SerializeField] private GameObject model;
	[SerializeField] private GameObject drop;
	[SerializeField] private GameObject breakFx;
    private float MyTimer = 0f;

	private float healthRemaining;

    // Start is called before the first frame update
    void Start() {
		healthRemaining = hitsToBreak;
    }

    public override void RegisterHit() {
        Instantiate(objectBreakParticles, transform.position, Quaternion.identity);

		healthRemaining--;

		if (healthRemaining <= 0 && model) {
			GameObject d = Instantiate(drop, locator.transform);
			d.transform.localPosition = Vector3.zero;

			d.transform.SetParent(null);

			GameObject p = Instantiate(breakFx, locator.transform);
			p.transform.localPosition = Vector3.zero;

			p.transform.SetParent(null);

			Destroy(model);
			GetComponent<BoxCollider>().enabled = false;
		}
    }
}
