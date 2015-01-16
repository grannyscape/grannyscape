using UnityEngine;
using System.Collections;

namespace grannyscape
{
	public class MainMenu : MonoBehaviour 
	{
		public float waitSeconds = 0.5f;

		public GameObject menu;
		public GameObject credits;

		public GameObject loader1;
		public GameObject loader2;
		public GameObject loader3;

		public AudioClip fart;
		
		void Start()
		{
			loader2.SetActive (false);
			loader3.SetActive (false);

			menu.SetActive(false);
			credits.SetActive(false);

			StartCoroutine ("Loader");
		}

		IEnumerator Loader()
		{
			yield return new WaitForSeconds(waitSeconds);
			loader1.SetActive(false);
			loader2.SetActive(true);
			yield return new WaitForSeconds(waitSeconds);
			loader2.SetActive(false);
			loader3.SetActive(true);
			yield return new WaitForSeconds(waitSeconds + 0.2f);
			loader3.SetActive(false);
			menu.SetActive(true);
		}

		public void StartGame()
		{
			StartCoroutine ("StartFart");
		}

		IEnumerator StartFart()
		{
			AudioSource.PlayClipAtPoint (fart, Camera.main.transform.position);
			yield return new WaitForSeconds (0.5f);
			Application.LoadLevel(1);
		}

		public void Credits()
		{
			AudioSource.PlayClipAtPoint (fart, Camera.main.transform.position);
			menu.SetActive (false);
			credits.SetActive(true);
		}

		public void Back()
		{
			AudioSource.PlayClipAtPoint (fart, Camera.main.transform.position);
			credits.SetActive(false);
			menu.SetActive (true);
		}

		public void Quit()
		{
			StartCoroutine ("QuitFart");
		}

		IEnumerator QuitFart()
		{
			AudioSource.PlayClipAtPoint (fart, Camera.main.transform.position);
			yield return new WaitForSeconds (0.5f);
			Application.Quit();
		}
	}

}