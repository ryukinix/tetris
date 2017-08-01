using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    // this is the button panels
    public GameObject[] buttons;
    public GameObject highScorePanel;
    public Text highscoreText;
    private int buttonSelected;
    private int numberOfButtons;

    void Awake() {
        numberOfButtons = buttons.Length;
        buttonSelected = 0;
        SelectNewGame();
        if (Highscore.highscore > 0) {
            highscoreText.text = Highscore.Get();
            highScorePanel.SetActive(true);
        }
    }

	public void NewGame() {
		SceneManager.LoadScene(1);
	}

    public void Exit() {
        Application.Quit ();
    }

    void openSelected() {
        if (buttonSelected == 0) {
            NewGame();
        } else if (buttonSelected == 1) {
            Exit();
        }
    }

    public void SelectNewGame() {
        buttons[0].SetActive(true);
        buttons[1].SetActive(false);
        buttonSelected = 0;
    }

    public void SelectExitGame() {
        buttons[1].SetActive(true);
        buttons[0].SetActive(false);
        buttonSelected = 1;
    }

    void changePanel(int direction) {
        buttons[buttonSelected].SetActive(false);
        //Debug.LogFormat("Old selected: {0}", buttonSelected);
        buttonSelected = Utils.Mod(buttonSelected + direction,  numberOfButtons);
        //Debug.LogFormat("New selected: {0}", buttonSelected);
        buttons[buttonSelected].SetActive(true);
    }

    public void Update() {
        // arrow browsing menu
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            changePanel(-1); // up
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            changePanel(1); // down
        } else if (Input.GetKeyDown(KeyCode.Return)) {
            openSelected(); // open selected button
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit(); // quit
        }
    }

}
