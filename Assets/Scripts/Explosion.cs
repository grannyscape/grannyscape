using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class Explosion : MonoBehaviour 
	{
		private Animator m_animator;

		void Start () 
		{
			m_animator = GetComponent<Animator>();
			StartCoroutine("Die");
		
		}

		IEnumerator Die()
		{
			//Todo: find animation length
			yield return new WaitForSeconds(0.5f);
			Destroy(gameObject);
		}
	}

}
