using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu
{
	public class CheckMusicVolume : MonoBehaviour
	{
		public void Start()
		{
			// remember volume level from last time
			Application.targetFrameRate = 90;
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
		}

		public void UpdateVolume()
		{
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
		}
	}
}