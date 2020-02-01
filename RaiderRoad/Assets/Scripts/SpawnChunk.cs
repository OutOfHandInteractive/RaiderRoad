using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChunk : MonoBehaviour {
    //--------------------
    // Public Variables
    //--------------------
    public List<GameObject> roadChunks;
    public float speed = -35.0f;
    public GameObject warningSprite;
    public Transform warningCamvas;
    public GameObject rv;
    public float spawnDespawnZDistance = 100.0f;

    //--------------------
    // Private Variables
    //--------------------
    private int nextChunk;
    private List<GameObject> warnings = new List<GameObject>();
    private float rvZStart;
    private BoxCollider col;
	[SerializeField] private List<GameObject> railPrefabs;
	[SerializeField] private List<GameObject> roadPrefabs;
    //road decals
    [SerializeField] private List<Texture> roadDecals;
    [SerializeField] private float decalSpawnChance; // 0.00 - 1.00 (percentage)
    [SerializeField] private Vector2 xOffsetMinMax;
    [SerializeField] private Material myMat;
    private int lastDecal = 0;

    [SerializeField] private List<GameObject> sceneryPrefabs;
	[SerializeField] private int sceneryChance = 20;
	[SerializeField] private Vector3 leftRailPos, rightRailPos;
	[SerializeField] private Vector3 leftRailRot, rightRailRot;

	// Use this for initialization
	void Start () {
        rvZStart = rv.transform.position.z;
        col = gameObject.GetComponent<BoxCollider>();

		// Spawn First Chunk
		GameObject road = Instantiate(roadChunks[0], transform.position, roadChunks[0].transform.rotation);
        road.GetComponent<MoveChunk>().SetSpeed(speed);
        road.GetComponent<MoveChunk>().SetSpawnDespawnZDistance(spawnDespawnZDistance);

		// give chunk road
		Instantiate(SelectRoad(), road.transform);

		// give chunk rails
		// left
		GameObject rail = Instantiate(SelectRail(), road.transform);
		rail.transform.localPosition = leftRailPos;
		rail.transform.rotation = Quaternion.Euler(leftRailRot);
		// right
		rail = Instantiate(SelectRail(), road.transform);
		rail.transform.localPosition = rightRailPos;
		rail.transform.rotation = Quaternion.Euler(rightRailRot);
	}

    // Spawning
    /// <summary>
    /// Spawns chunk at a location.
    /// </summary>
    /// <param name="spawnLocation">Chunk spawn location.</param>
    public void Spawn(Vector3 spawnLocation) {
        // Spawn Chunks in Random Order
        int rand = Random.Range(0, roadChunks.Count);
        GameObject road = Instantiate(roadChunks[rand], spawnLocation, roadChunks[rand].transform.rotation);
        road.GetComponent<MoveChunk>().SetSpeed(speed);
        road.GetComponent<MoveChunk>().SetSpawnDespawnZDistance(spawnDespawnZDistance);

		// give chunk road
		Instantiate(SelectRoad(), road.transform);

		// give chunk rails
		// left
		GameObject rail = Instantiate(SelectRail(), road.transform);
		rail.transform.localPosition = leftRailPos;
		rail.transform.rotation = Quaternion.Euler(leftRailRot);
		// right
		rail = Instantiate(SelectRail(), road.transform);
		rail.transform.localPosition = rightRailPos;
		rail.transform.rotation = Quaternion.Euler(rightRailRot);

		// scenery
		Instantiate(SelectScenery(), road.transform);

        //give road decal (or not)
        if(Random.value <= decalSpawnChance){
            int myDec = Random.Range(0, roadDecals.Count-1);
            if(myDec >= lastDecal) {
                myDec = (myDec + 1) % roadDecals.Count;
            }
            if(myDec >= roadDecals.Count)
            {
                Debug.Log("Decal index out of bounds??");
                myDec = 0;
            }

            myMat = Instantiate(myMat);
            myMat.SetTexture("_OverTex", roadDecals[myDec]);
            float offset = Random.Range(xOffsetMinMax.x, xOffsetMinMax.y);
            myMat.SetTextureOffset("_OverTex", new Vector2(offset, 0.15f));
            Debug.Log("DAB DAB DAB");//road.transform.GetChild(2));
            road.transform.GetChild(2).GetComponent<Renderer>().material = myMat;
            lastDecal = myDec; //make sure it doesn't repeat
        }
	}

    #region Component Selection
    /// <summary>
    /// Selects the Road type from a road prefab list.
    /// </summary>
    private GameObject SelectRoad() {
		int rand = Random.Range(0, roadPrefabs.Count);
		return roadPrefabs[rand];
	}

    /// <summary>
    /// Selects the Rail type from a rail prefab list.
    /// </summary>
	private GameObject SelectRail() {
		int rand = Random.Range(0, railPrefabs.Count);
		return railPrefabs[rand];
	}

    /// <summary>
    /// Selects the Scenery from a scenery prefab list.
    /// </summary>
	private GameObject SelectScenery() {
		int rand = Random.Range(1, 101);
		if (rand <= sceneryChance) {
			rand = Random.Range(1, sceneryPrefabs.Count);
			return sceneryPrefabs[rand];
		}
		else {
			return sceneryPrefabs[0];
		}
	}
	#endregion

	#region Detection and Trigger Functions
	// Warning Arrows
	void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Obstacle")) {
            warnings.Add(Instantiate(warningSprite, warningCamvas));
            warnings[warnings.Count - 1].GetComponent<UiElementFollowObject>().SetObject(other.gameObject);
            warnings[warnings.Count - 1].GetComponent<UiElementFollowObject>().SetCanvas(warningCamvas.GetComponent<RectTransform>());
            warnings[warnings.Count - 1].GetComponent<UiElementFollowObject>().SetRv(rv);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals("Obstacle")) {
            Destroy(warnings[0]);
            warnings.RemoveAt(0);
        }
    }
	#endregion
}
