using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
    public Object[] BGM;
    public AudioSource audioSource;
    public static AudioSource source;
    private bool tryingChange = false;
    public static int nextMusic;

    void Awake() {
        if (source == null) {
            source = audioSource;
            DontDestroyOnLoad(this); // to no restart music on a new game
            randomInitialization();
            source.Play();
        }
    }

    // get the first music by random number
    void randomInitialization() {
        nextMusic = Random.Range(0, BGM.Length);
        source.clip = BGM[nextMusic] as AudioClip;
    }

    // select next music and increment nextMusic by circular reference
    void selectNextMusic(){
        source.clip = BGM[nextMusic] as AudioClip;
        nextCircularPlaylist();
    }

    // Create a circular reference: 0 1 2 0 1 2 0 1 2 ...
    void nextCircularPlaylist() {
        nextMusic = Utils.Mod(nextMusic + 1, BGM.Length);
    }

    // Select the next music and play it
    void playNextMusic() {
        selectNextMusic();
        source.Play();
    }
   
    // this avoid the behavior of start a new music
    // when the unity stop by desfocusing
    // So we wait for 1 second before change for new music
    // after isPLaying is false
    IEnumerator tryChange() {
        tryingChange = true;
        yield return new WaitForSeconds(1);
        if (!source.isPlaying) {
            playNextMusic();
        }
        tryingChange = false;
    }
   
	// Update is called once per frame
	void Update () {
        if (!tryingChange && !source.isPlaying) {
            StartCoroutine(tryChange());
        }
	}
}
