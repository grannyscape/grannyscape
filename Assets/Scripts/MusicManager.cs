using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace grannyscape
{
	public enum Music
	{
		NONE,
		MENU,
		GAME,
		WIN
	}
	
	public class MusicManager : MonoBehaviour 
	{
		public AudioClip menuMusic;
		public AudioClip gameMusic;
		public AudioClip winMusic;

		private Music m_currentMusic;
		private AudioSource m_audioSource;
		private Dictionary<int, AudioClip> m_clips = new Dictionary<int, AudioClip>();


		// Use this for initialization
		void Start () 
		{
			m_audioSource = GetComponent<AudioSource> ();


			m_clips.Add ((int)Music.MENU, menuMusic);
			m_clips.Add ((int)Music.GAME, gameMusic);
			m_clips.Add ((int)Music.WIN, winMusic);
		}

		public void SetPitch(float pitch)
		{
			m_audioSource.pitch = pitch;
		}

		public void PlayMusic(Music clip)
		{
			if(clip != m_currentMusic)
			{
				if(clip == Music.WIN)
				{
					m_audioSource.loop = false;
				}
				else
				{
					m_audioSource.loop = true;
				}

				m_audioSource.clip = m_clips[(int)clip];
				m_audioSource.Play ();
				m_currentMusic = clip;
			}
		}
	}

}