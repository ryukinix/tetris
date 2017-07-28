using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
    public Object[] BGM;
    public AudioSource audioSource;

    void Awake() {
        selectRandomMusic();
    }

    void selectRandomMusic(){
        int idx = Random.Range(0, BGM.Length);
        audioSource.clip = BGM[idx] as AudioClip;
    }

    void playRandomMusic() {
        selectRandomMusic();
        audioSource.Play();
    }
   
    void stopMusic() {
        audioSource.Stop();
    }

	// Use this for initialization
	void Start () {
        audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (!audioSource.isPlaying) {
            playRandomMusic();
        }
	}
}
