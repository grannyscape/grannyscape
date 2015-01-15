using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace grannyscape
{
	public enum Music
	{
		MENU,
		GAME,
		WIN
	}
	
	public class MusicManager : MonoBehaviour 
	{
		public AudioClip menuMusic;
		public AudioClip gameMusic;
		public AudioClip winMusic;

		private AudioSource m_audioSource;
		private Dictionary<int, AudioClip> m_clips;


		// Use this for initialization
		void Start () 
		{
			m_audioSource = GetComponent<AudioSource> ();
			m_audioSource.loop = true;

			//m_clips.Add (Music.MENU, menuMusic);
			//m_clips.Add (Music.GAME, gameMusic);
			//m_clips.Add (Music.WIN, winMusic);
		}
		
		public void playMusic(Music clip)
		{
			//m_audioSource.Play (m_clips [clip]);
		}
	}

}