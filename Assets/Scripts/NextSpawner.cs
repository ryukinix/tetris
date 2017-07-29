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

    // code borrowed from: 
    // https://forum.unity3d.com/threads/getting-the-bounds-of-the-group-of-objects.70979/
    Bounds getRenderBounds(GameObject obj){
        var bounds = new  Bounds(Vector3.zero,Vector3.zero);
        var render = obj.GetComponent<Renderer>();
        return render != null? render.bounds : bounds;
    }

    // this too
    Bounds getBounds(GameObject obj){
        Bounds bounds;
        Renderer childRender;
        bounds = getRenderBounds(obj);
        if((int)bounds.extents.x == 0){
            bounds = new Bounds(obj.transform.position,Vector3.zero);
            foreach (Transform child in obj.transform) {
                childRender = child.GetComponent<Renderer>();
                if (childRender) {
                    bounds.Encapsulate(childRender.bounds);
                }else{
                    bounds.Encapsulate(getBounds(child.gameObject));
                }
            }
        }
        return bounds;
    }


    void createStoppedGroup () {
        currentGroupObject = spawner.createGroup(transform.position);
        currentGroupId = spawner.nextId;
        // Debug.LogFormat("Object position: {0}", currentGroupObject.transform.position);
        // Debug.LogFormat("Center position: {0}", getBounds(currentGroupObject).center);
        var group = (Group) currentGroupObject.GetComponent(typeof(Group));
        // put the group align with its center
        group.transform.position += transform.position - getBounds(currentGroupObject).center;
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
