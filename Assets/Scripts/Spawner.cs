using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] groups;
    public int nextId;

	// Use this for initialization
	void Start () {
        nextId = Random.Range(0, groups.Length);
        spawnNext ();
	}
	
	// Update is called once per frame
	void Update () {
		// nothing
	}

    public GameObject createGroup(Vector3 v) {
        GameObject group = Instantiate(groups[nextId], v, Quaternion.identity);
        //group.transform.SetParent(GameObject.FindGameObjectWithTag("Board").transform);
        //group.transform.position *= canvas.scaleFactor; bug bug bug everywhere
        // solved in another way: just adjust the UI HUD to scale and keep this shit constant
        return group;
    }

	// spawnNext group block
	public void spawnNext() {
		// Spawn Group at current Position
        createGroup(transform.position);
        nextId = Random.Range(0, groups.Length);
	}
}
