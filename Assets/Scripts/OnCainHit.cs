using UnityEngine;
using System.Collections;

namespace grannyscape
{
	public class OnCainHit : MonoBehaviour 
	{
		public PlayerController pc;

		public void OnActionCainHit()
		{
			pc.OnActionCainHit();
		}
	}
}