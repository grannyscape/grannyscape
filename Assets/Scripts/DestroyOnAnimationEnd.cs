using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class DestroyOnAnimationEnd : MonoBehaviour 
	{

		public void OnAnimationEnded()
		{
			Destroy (gameObject);
		}
	}

}
