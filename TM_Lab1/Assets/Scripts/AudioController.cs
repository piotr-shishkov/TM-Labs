using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	public AudioSource SoundAudioSource;
	public List<GameSound> Sounds = new List<GameSound>();

	public void PlaySound(string soundName)
	{
		var sound = Sounds.FirstOrDefault(s => s.SoundName == soundName);
		if (sound != null) SoundAudioSource.PlayOneShot(sound.SoundClip);
		else Debug.LogWarning($"Sounds does not contains sound with name: ${soundName}");
	}

	public void PlaySound(int soundIndex)
	{
		var sound = Sounds[soundIndex];
		if (sound != null) SoundAudioSource.PlayOneShot(sound.SoundClip);
		else Debug.LogWarning($"Sounds does not contains sound with index: ${soundIndex}");
	}
}

[Serializable]
public class GameSound
{
	public string SoundName;
	public AudioClip SoundClip;
}
