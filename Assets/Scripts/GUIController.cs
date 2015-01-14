using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour 
{
	public GameObject player;
	
	public Slider healthBarSlider;  	
	public Text gameOverText;   	
	private bool m_isGameOver = false; 
	
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!m_isGameOver) 
		{
			healthBarSlider.value -= .001f;  //reduce health
			
			if (healthBarSlider.value < 0) 
			{
				m_isGameOver = true;
			}
		}
	}

	public void SetHealth(float health)
	{
		healthBarSlider.value = health;
	}
}
