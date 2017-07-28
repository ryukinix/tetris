using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static int level = 1;

    Text levelText;

    // Use this for initialization
    void Awake () {
        levelText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        if (ScoreManager.score > level * 1000) {
            level += 1;
        }
        levelText.text = level.ToString();
    }
}
