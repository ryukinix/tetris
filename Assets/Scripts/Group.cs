using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Group : MonoBehaviour {

    // time of the last fall, used to auto fall after 
    // time parametrized by `level`
    private float lastFall = 0;
    private float lastKeyDown = 0;

    /// control if the user can generate a new block 
    public static bool died = false;

    bool isValidGridPos() {
        foreach (Transform child in transform) {
            Vector2 v = Grid.roundVector2(child.position);

            // not inside Border?
            if(!Grid.insideBorder(v)) {
                return false;
            }

            // Block in grid cell (and not par of same group)?
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform) {
                return false;
            }
        }

        return true;
    }

    // update the grid
    void updateGrid() {
        // Remove old children from grid
        for (int y = 0; y < Grid.h; y++) {
            for (int x = 0; x < Grid.w; x++) {
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

	// Use this for initialization
	void Start () {
        // default position not valid? then it's game over
        if (!isValidGridPos()) {
            Debug.Log("GAME OVER!");
            updateGrid();
            died = true;
        }
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
            if (isValidGridPos()){
                // it's valid. Update grid
                updateGrid();
            } else {
                // it's not valid. revert
                transform.Rotate(0, 0, 90);
            }
        } else if (Input.GetKeyDown(KeyCode.DownArrow) 
                   || Input.GetKey(KeyCode.DownArrow) && Time.time - lastKeyDown > 0.75 &&  Time.time - lastFall > .05
                   || (Time.time - lastFall) >= (float) 1/Mathf.Sqrt(LevelManager.level)) {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                lastKeyDown =  Time.time;
            }

            // modify
            transform.position += new Vector3(0, -1, 0);

            if (isValidGridPos()){
                 // It's valid. Update grid... again
                updateGrid();
            } else {
                // it's not valid. rvert
                transform.position += new Vector3(0, 1, 0);

                // Clear filled horizontal lines
                Grid.deleteFullRows();

                // Spawn next Group if not died
                if (!died) {
                    FindObjectOfType<Spawner>().spawnNext();
                }

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }

	}
}
