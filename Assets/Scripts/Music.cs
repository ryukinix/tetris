using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
    public Object[] BGM;
    public AudioSource audioSource;
    // all need this to stop the music from outside script
    // I don't know do better yet
    public static AudioSource source = null; // gamb
    public static bool stopped = false; // gamb

    void Awake() {
        DontDestroyOnLoad(this); // to no restart music on a new game
        if (source == null) {
            source = audioSource;
            selectRandomMusic();
            source.Play();
        }
    }

    void selectRandomMusic(){
        int idx = Random.Range(0, BGM.Length);
        source.clip = BGM[idx] as AudioClip;
    }

    void playRandomMusic() {
        selectRandomMusic();
        source.Play();
    }
   
    public static void stopMusic() {
        source.Stop();
        stopped = true;
    }

	// Use this for initialization
	void Start () {
        //source.Play();
	}

	// Update is called once per frame
	void Update () {
        if (!source.isPlaying && !stopped) {
            playRandomMusic();
        }
	}
}
