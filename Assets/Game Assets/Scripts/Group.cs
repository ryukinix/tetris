using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Group : MonoBehaviour {

    // time of the last fall, used to auto fall after 
    // time parametrized by `level`
    private float lastFall;

    // last key pressed time, to handle long press behavior
    private float lastKeyDown;
    private float timeKeyPressed;

    public void AlignCenter() {
        transform.position += transform.position - Utils.Center(gameObject);
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

        insertOnGrid();
    }

    void insertOnGrid() {
        // add new children to grid
        foreach (Transform child in transform) {
            Vector2 v = Grid.roundVector2(child.position);
            Grid.grid[(int)v.x,(int)v.y] = child;
        }
    }

    void gameOver() {
        Debug.Log("GAME OVER!");
        while (!isValidGridPos()) {
            //Debug.LogFormat("Updating last group...: {0}", transform.position);
            transform.position  += new Vector3(0, 1, 0);
        } 
        updateGrid(); // to not overleap invalid groups
        enabled = false; // disable script when dies
        UIController.gameOver(); // active Game Over panel
        Highscore.Set(ScoreManager.score); // set highscore
        //Music.stopMusic(); // stop Music
    }

    // Use this for initialization
    void Start () {
        lastFall = Time.time;
        lastKeyDown = Time.time;
        timeKeyPressed = Time.time;
        if (isValidGridPos()) {
            insertOnGrid();
        } else { 
            Debug.Log("KILLED ON START");
            gameOver();
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

    void fallGroup() {
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


            FindObjectOfType<Spawner>().spawnNext();


            // Disable script
            enabled = false;
        }

        lastFall = Time.time;

    }

    // getKey if is pressed now on longer pressed by 0.5 seconds | if that true apply the key each 0.05f while is pressed
    bool getKey(KeyCode key) {
        bool keyDown = Input.GetKeyDown(key);
        bool pressed = Input.GetKey(key) && Time.time - lastKeyDown > 0.5f && Time.time - timeKeyPressed > 0.05f;

        if (keyDown) {
            lastKeyDown = Time.time;
        }
        if (pressed) {
            timeKeyPressed = Time.time;
        }
 
        return keyDown || pressed;
    }


    // Update is called once per frame
    void Update () {
        if (UIController.isPaused) {
            return; // don't do nothing
        }
        if (getKey(KeyCode.LeftArrow)) {
            tryChangePos(new Vector3(-1, 0, 0));
        } else if (getKey(KeyCode.RightArrow)) {  // Move right
            tryChangePos(new Vector3(1, 0, 0));
        } else if (getKey(KeyCode.UpArrow) && gameObject.tag != "Cube") { // Rotate
            transform.Rotate(0, 0, -90);

            // see if valid
            if (isValidGridPos()) {
                // it's valid. Update grid
                updateGrid();
            } else {
                // it's not valid. revert
                transform.Rotate(0, 0, 90);
            }
        } else if (getKey(KeyCode.DownArrow) || (Time.time - lastFall) >= (float)1 / Mathf.Sqrt(LevelManager.level)) {
            fallGroup();
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            while (enabled) { // fall until the bottom 
                fallGroup();
            }
        }

    }
}
