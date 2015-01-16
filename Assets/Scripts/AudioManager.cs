using UnityEngine;
using System.Collections;

namespace grannyscape
{	
	public enum SoundType
	{
		POWERUP_COIN,
		POWERUP_COFFEE,
		POWERUP_SPEEDPILL,
		POWERUP_SLOWPILL,
		POWERUP_PEASOUP,
		GRANNY_FART,
		GRANNY_JUMP,
		GRANNY_JUMP_LANDING,
		GRANNY_LOSE,
		GRANNY_PILL_EFFECT_ON,
		GRANNY_PILL_EFFECT_OFF,
		GRANNY_STICK_SWING,
		GRANNY_STICK_HIT,
		GRANNY_WALKING,
		GRANNY_WIN,
		PUNK_HIT,
		AMBULANCE,
	}

	public class AudioManager : MonoBehaviour 
	{
		public AudioClip[] powerupCoin;
		public AudioClip[] powerupCoffee;
		public AudioClip[] powerupSpeedpill;
		public AudioClip[] powerupSlowpill;
		public AudioClip[] powerupPeasoup;
		public AudioClip[] grannyFart;
		public AudioClip[] grannyJump;
		public AudioClip[] grannyLanding;
		public AudioClip[] grannyLose;
		public AudioClip[] grannyPillEffectOn;
		public AudioClip[] grannyPillEffectOff;
		public AudioClip[] grannyStickSwing;
		public AudioClip[] grannyStickHit;
		public AudioClip[] grannyWalking;
		public AudioClip[] grannyWin;
		public AudioClip[] punkHit;
		public AudioClip[] ambulance;

		private float m_pitch = 1f;

		public void SetPitch(float pitch)
		{
			m_pitch = pitch;
		}

		public void PlaySound(SoundType type)
		{
			AudioClip[] clips = null;

			switch(type)
			{
			case SoundType.POWERUP_COIN:
				clips = powerupCoin;
				break;
			case SoundType.POWERUP_COFFEE:
				clips = powerupCoffee;
				break;
			case SoundType.POWERUP_SPEEDPILL:
				clips = powerupSpeedpill;
				break;
			case SoundType.POWERUP_SLOWPILL:
				clips = powerupSlowpill;
				break;
			case SoundType.POWERUP_PEASOUP:
				clips = powerupPeasoup;
				break;
			case SoundType.GRANNY_FART:
				clips = grannyFart;
				break;
			case SoundType.GRANNY_JUMP:
				clips = grannyJump;
				break;
			case SoundType.GRANNY_JUMP_LANDING:
				clips = grannyLanding;
				break;
			case SoundType.GRANNY_LOSE:
				clips = grannyLose;
				break;
			case SoundType.GRANNY_PILL_EFFECT_ON:
				clips = grannyPillEffectOn;
				break;
			case SoundType.GRANNY_PILL_EFFECT_OFF:
				clips = grannyPillEffectOff;
				break;
			case SoundType.GRANNY_STICK_SWING:
				clips = grannyStickSwing;
				break;
			case SoundType.GRANNY_STICK_HIT:
				clips = grannyStickHit;
				break;
			case SoundType.GRANNY_WALKING:
				clips = grannyWalking;
				break;
			case SoundType.GRANNY_WIN:
				clips = grannyWin;
				break;
			case SoundType.PUNK_HIT:
				clips = punkHit;
				break;
			case SoundType.AMBULANCE:
				clips = ambulance;
				break;
			}

			if(clips != null && clips.Length > 0)
			{
				PlayClipAt(clips[Random.Range (0, clips.Length-1)], Camera.main.transform.position);
			}
			//AudioSource.PlayClipAtPoint (clips[Random.Range (0, clips.Length-1)], Camera.main.transform.position);
		}
	
		AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
		{
			GameObject tempGO = new GameObject("TempAudio"); 
			tempGO.transform.position = pos; 
			AudioSource aSource = tempGO.AddComponent<AudioSource>(); 
			aSource.clip = clip;
			aSource.pitch = m_pitch;

			aSource.Play(); 
			Destroy(tempGO, clip.length); 
			return aSource;
		}

	}

}
