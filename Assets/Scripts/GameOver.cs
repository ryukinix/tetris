using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour {
    public static string message = "";
    public static bool finished = false;
    public Text text;
 	// Use this for initialization
    void Start () {
	}
	
    // restart the game 
    void restart() {
        Debug.Log("RESTART GAME!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // terminate the game
    public static void finish() {
        setMessage();
        finished = true;
    }

    public static void setMessage() {
        // set message Game Over to print on screen
        message = "GAME OVER";
    }

	// Update is called once per frame
	void Update () {
        if (message != "") {
            text.text = message;
        } 

        // exit the game if presed ESC
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        } else if (finished && Input.anyKeyDown) {
            finished = false;
            restart();
            message = "";
            text.text = message;
        }
	}
}
