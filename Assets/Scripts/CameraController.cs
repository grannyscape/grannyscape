using UnityEngine;
using System.Collections;

namespace grannyscape
{

public class CameraController : MonoBehaviour 
{
	public GameObject player;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void LateUpdate()
	{
		Vector3 pos = transform.position;
		pos.x = player.transform.position.x;
		transform.position = pos;
	}
}

}