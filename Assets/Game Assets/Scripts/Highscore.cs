using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {

    public static int highscore = 0;

    public static void Set(int score) {
        if (score > highscore) {
            highscore = score;
        }
        
    }

    public static string Get() { 
        return System.String.Format("{0:D8}", highscore);
    }

}
