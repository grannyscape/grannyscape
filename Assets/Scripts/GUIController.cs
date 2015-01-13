using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour 
{
	public GUIText velocity;

	public GameObject player;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		velocity.text = "Velocity: " + player.rigidbody2D.velocity.x;
	}
}
