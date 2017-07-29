using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Group : MonoBehaviour {

    // time of the last fall, used to auto fall after 
    // time parametrized by `level`
    private float lastFall;

    // last key pressed time, to handle long press behavior
    private float lastKeyDown;

    // code borrowed from: 
    // https://forum.unity3d.com/threads/getting-the-bounds-of-the-group-of-objects.70979/
    public static Bounds getRenderBounds(GameObject obj){
        var bounds = new  Bounds(Vector3.zero,Vector3.zero);
        var render = obj.GetComponent<Renderer>();
        return render != null? render.bounds : bounds;
    }

    // this too
    public static Bounds getBounds(GameObject obj){
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

    public Vector3 Center () {
        return getBounds(gameObject).center;
    }

    public void AlignCenter() {
        transform.position += transform.position - Center();
    }


    bool isValidGridPos() {
        foreach (Transform child in transform) {
            Vector2 v = Grid.roundVector2(child.position);

            // not inside Border?
            if(!Grid.insideBorder(v)) {
                return false;
            }

            // Block in grid cell (and not par of same group)?
            if (Grid.grid[(int)(v.x), (int)(v.y)] != null &&
                Grid.grid[(int)(v.x), (int)(v.y)].parent != transform) {
                return false;
            }
        }

        return true;
    }

    // update the grid
    void updateGrid() {
        // Remove old children from grid
        for (int y = 0; y < Grid.h; ++y) {
            for (int x = 0; x < Grid.w; ++x) {
                if (Grid.grid[x,y] != null &&
                    Grid.grid[x,y].parent == transform) {
                    Grid.grid[x,y] = null;
                }
            } 
        }

        // add new children to grid
        foreach (Transform child in transform) {
            Vector2 v = Grid.roundVector2(child.position);
            Grid.grid[(int)v.x,(int)v.y] = child;
        }
    }

    void gameOver() {
        Debug.Log("GAME OVER!");
        do {
            Debug.LogFormat("Updating last group...: {0}", transform.position);
            transform.position  += new Vector3(0, 1, 0);
        } while (!isValidGridPos());
        updateGrid(); // to not overleap invalid groups
        enabled = false; // disable script when dies
        UIController.gameOver(); // active Game Over panel
        //Music.stopMusic(); // stop Music
    }

	// Use this for initialization
	void Start () {
        // default position not valid? then it's game over
        if (!isValidGridPos()) {
            gameOver();
        }
        lastFall = Time.time;
        lastKeyDown = Time.time;
	}

    void tryChangePos(Vector3 v) {
        // modify position 
        // FIXME: maybe this is idiot? I can create a copy before and only assign to the transform if is valid
        transform.position += v;

        // See if valid
        if (isValidGridPos()) {
            updateGrid();
        } else {
            transform.position -= v;
        }
    }

    void fallGroup() {

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            lastKeyDown =  Time.time;
        }

        // modify
        transform.position += new Vector3(0, -1, 0);

        if (isValidGridPos()){
            // It's valid. Update grid... again
            updateGrid();
        } else {
            // it's not valid. revert
            transform.position += new Vector3(0, 1, 0);

            // Clear filled horizontal lines
            Grid.deleteFullRows();

            // Spawn next Group if not died
            updateGrid();
            FindObjectOfType<Spawner>().spawnNext();

            // Disable script
            enabled = false;
        }

        lastFall = Time.time;
    
    
    }
        
	    
	// Update is called once per frame
	void Update () {
		// Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            tryChangePos(new Vector3(-1, 0, 0));
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {  // Move right
            tryChangePos(new Vector3(1, 0, 0));
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) { // Rotate
            transform.Rotate(0, 0, -90);

            // see if valid
            if (isValidGridPos()) {
                // it's valid. Update grid
                updateGrid();
            } else {
                // it's not valid. revert
                transform.Rotate(0, 0, 90);
            }
        } else if (Input.GetKeyDown(KeyCode.DownArrow)
                   || Input.GetKey(KeyCode.DownArrow) && Time.time - lastKeyDown > 0.75 && Time.time - lastFall > .05
                   || (Time.time - lastFall) >= (float)1 / Mathf.Sqrt(LevelManager.level)) {
            fallGroup();
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            while (enabled) { // fall until the bottom 
                fallGroup();
            }
        }

	}
}
