using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class PlayerController : MonoBehaviour 
	{
		public float moveSpeed = 4f;
		public float minSpeed = 2f;
		public float maxSpeed = 8f;
		public float jumpForce = 1000f;
		
		private bool m_bJump = false;
		private bool m_bGrounded = false;
		private bool m_bSlidingDown = false;

		private bool m_bFrontCollisionUp = false;
		private bool m_bFrontCollisionDown = false;

		private Transform m_groundCheck;
		private Transform m_frontCheckUp;
		private Transform m_frontCheckDown;

		private float m_lastPositionY = 0;
		
		void Awake()
		{
			m_groundCheck = transform.Find("groundCheck");
			m_frontCheckUp = transform.Find("frontCheckUp");
			m_frontCheckDown = transform.Find("frontCheckDown");
		}

		// Use this for initialization
		void Start () 
		{
		}
		
		// Update is called once per frame
		void Update () 
		{
			// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
			m_bGrounded = Physics2D.Linecast(transform.position, m_groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

			// Check front collisions
			m_bFrontCollisionUp = Physics2D.Linecast(transform.position, m_frontCheckUp.position, 1 << LayerMask.NameToLayer("Ground"));
			m_bFrontCollisionDown = Physics2D.Linecast(transform.position, m_frontCheckDown.position, 1 << LayerMask.NameToLayer("Ground"));
			
			if(Input.GetButtonDown ("Jump") && m_bGrounded)
			{
				m_bJump = true;
			}
		}

		void FixedUpdate () 
		{
			float dt = Time.deltaTime;

			if(!m_bSlidingDown)
			{
				if (!m_bFrontCollisionUp && !m_bFrontCollisionDown) 
				{
					transform.Translate (transform.right * moveSpeed * dt);
				} 
				else 
				{
					m_bSlidingDown = true;
					m_lastPositionY = transform.position.y;
				}
			}

			if (m_bSlidingDown) 
			{
				if(m_lastPositionY >= transform.position.y)
				{
					//transform.Translate (-transform.right * minSpeed * dt);
				}
				m_lastPositionY = transform.position.y;
			}

			if (m_bGrounded) 
			{
				m_bSlidingDown = false;
			}

			if(m_bJump)
			{
				rigidbody2D.AddForce(new Vector2(0f, jumpForce));
				m_bJump = false;
			}
		}

		void OnDrawGizmos() 
		{
			if (Application.isPlaying) 
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine (transform.position, m_groundCheck.transform.position);
				Gizmos.DrawLine (transform.position, m_frontCheckUp.transform.position);
				Gizmos.DrawLine (transform.position, m_frontCheckDown.transform.position);
			}
		}

		/*
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
		*/
		
	}
}
