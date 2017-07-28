using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] groups;
    public static int nextId;

	// Use this for initialization
	void Start () {
		spawnNext ();
        nextId = Random.Range(0, groups.Length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject createBlock(int i, Vector3 v) {
        return Instantiate(groups[i], v, Quaternion.identity);
    }

	// spawnNext group block
	public void spawnNext() {
		// Spawn Group at current Position
        createBlock(nextId, transform.position);
	   
        nextId = Random.Range(0, groups.Length);
	}
}
