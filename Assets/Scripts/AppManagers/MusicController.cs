using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
	public static MusicController instance;

	public AudioClip[] audioClips;

	[HideInInspector]
	public AudioSource audioSource;

	void Awake(){
		MakeInstance ();
		audioSource = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		
	}
		
	// Update is called once per frame
	void Update () {
		
	}

	void MakeInstance(){
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	public void PlayBgMusic(){
		AudioClip bgMusic = audioClips [0];
		if(bgMusic){
			audioSource.clip = bgMusic;
			audioSource.loop = true;
			audioSource.Play ();
		}
	}

	public void StopBgMusic(){
		if(audioSource.isPlaying){
			audioSource.Stop ();
		}
	}

	public void PlayGameplayMusic(){
		AudioClip gameplayMusic = audioClips [1];
		if(gameplayMusic){
			audioSource.clip = gameplayMusic;
			audioSource.loop = true;
			audioSource.Play ();
		}
	}

}
