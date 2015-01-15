using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class MainMenu : MonoBehaviour 
	{
		public void StartGame()
		{
			//Application.LoadLevel(1);
		}

		public void Credits()
		{
			Debug.Log ("(c) Team GrannyScape 2015");
		}

		public void Quit()
		{
			Application.Quit ();
		}
	}

}