using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public Sound[] sounds;
    public List<Sound> currentSounds;

    // Use this for initialization
    void Awake () {
		foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
	}
	
	public void Play(string name) {
        Sound s = null;
        s = FindSound(name, s);
        if (s != null) {
            s.source.Play();
            currentSounds.Add(s);
        }
        else {
            Debug.LogError("Cannot find sound " + name);
        }
    }

    public void PlayLooping(string name) {
        Sound s = null;
        s = FindSound(name, s);
        if (s != null) {
            if (!s.source.isPlaying) {
                s.source.Play();
            }
        }
        else {
            Debug.LogError("Cannot find sound " + name);
        }
    }

    private Sound FindSound(string name, Sound s) {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == name) {
                s = sounds[i];
                break;
            }
        }

        return s;
    }
}
