using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSpawner : MonoBehaviour {

    private Spawner spawner;
    private GameObject currentGroupObject;
    private int currentGroupId; 

	// Use this for initialization
    void Awake () {
        spawner = FindObjectOfType<Spawner>();
	}

    void createStoppedGroup () {
        currentGroupObject = spawner.createGroup(transform.position);
        currentGroupId = spawner.nextId;
        var group = (Group) currentGroupObject.GetComponent(typeof(Group));
        group.enabled = false;
    }


    void deleteCurrentGroup() {
        Destroy(currentGroupObject);
    }

    void Start() {
        createStoppedGroup();
    }
	
	// Update is called once per frame
	void Update () {
        if (currentGroupId != spawner.nextId) {
            deleteCurrentGroup();
            createStoppedGroup();
        }
	}
}
