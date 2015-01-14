using UnityEngine;
using System.Collections;

namespace grannyscape
{

public class PlayerController : MonoBehaviour 
{
	public float moveForce = 365f;
	public float maxSpeed = 8f;
	public float jumpForce = 1000f;
	
	private bool m_jump = false;	
	private Transform m_groundCheck;
	private bool m_grounded = false;
	
	void Awake()
	{
		m_groundCheck = transform.Find("groundCheck");
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		m_grounded = Physics2D.Linecast(transform.position, m_groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		
		if(Input.GetButtonDown ("Jump"))
		{
			if(m_grounded)
			{
				Debug.Log ("should jump");
				m_jump = true;
			}
			else
			{
				Debug.Log ("Jumping, cannot jump more");
			}
		}
	}

	void FixedUpdate()
	{
		rigidbody2D.AddForce(Vector2.right * moveForce);
		//rigidbody2D.velocity.x = Vector2.ClampMagnitude(rigidbody2D.velocity.x, maxSpeed);

		Vector2 v = rigidbody2D.velocity;
		v.x = Mathf.Clamp(v.x, 0.0f, maxSpeed);
		rigidbody2D.velocity = v;

		// If the player should jump...
		if(m_jump)
		{
			Debug.Log ("adding jump force");
			// Add a vertical force to the player.
			//rigidbody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
			Vector2 vv = rigidbody2D.velocity;	
			vv.y = 200;
			rigidbody2D.velocity=vv;	
			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			m_jump = false;
		}
		
	}
	
}

}
