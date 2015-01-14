using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class PlayerController : MonoBehaviour 
	{
		public float moveSpeed = 4f;
		public float minSpeed = 2f;
		public float maxSpeed = 8f;
		public float jumpHeight = 2;
	
		private bool m_bJump = false;
		private bool m_bGrounded = false;
		private bool m_bFalling = false;

		private bool m_bFrontCollision = false;

		private Transform m_groundCheck;
		private Transform m_frontCheck;
	
		private float m_lastPositionY = 0;

		private Vector2 m_frontCheckStart = new Vector2(0f, 0f);
		
		void Awake()
		{
			m_groundCheck = transform.Find("groundCheck");
			m_frontCheck = transform.Find("frontCheck");
		}

		// Use this for initialization
		void Start () 
		{
			m_lastPositionY = transform.position.y;
		}
		
		// Update is called once per frame
		void Update () 
		{
			// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
			m_bGrounded = Physics2D.Linecast(transform.position, m_groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

			// Check front collision
			m_frontCheckStart.x = transform.position.x;
			m_frontCheckStart.y = m_frontCheck.position.y;
			m_bFrontCollision = Physics2D.Linecast(m_frontCheckStart, m_frontCheck.position, 1 << LayerMask.NameToLayer("Ground"));

			if(Input.GetButtonDown ("Jump") && m_bGrounded)
			{
				m_bJump = true;
			}

			// Check falling
			if (!m_bGrounded && m_lastPositionY != transform.position.y) 
			{
				m_bFalling = true;
			}
			else 
			{
				m_bFalling = false;
			}
		}

		void FixedUpdate () 
		{
			float dt = Time.deltaTime;

			if (!m_bFrontCollision)
			{
				//transform.Translate (transform.right * moveSpeed * dt);

				rigidbody2D.AddForce(Vector2.right * moveSpeed * 2);

				if(rigidbody2D.velocity.magnitude > maxSpeed)
				{
					Vector2 currentVelocity = rigidbody2D.velocity;
					currentVelocity = rigidbody2D.velocity.normalized * maxSpeed;
					rigidbody2D.velocity = currentVelocity;
				}
			} 


			if(m_bJump)
			{
				Debug.Log (jumpHeight);
				Debug.Log(Physics2D.gravity.y);

				float jumpForce = Mathf.Sqrt (2.0f * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
				Debug.Log (jumpForce);

				//rigidbody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
				Vector2 currentVelocity = rigidbody2D.velocity;
				currentVelocity.y = jumpForce;
				rigidbody2D.velocity = currentVelocity;

				Debug.Log ("done");
				m_bJump = false;
			}
		}

		void OnDrawGizmos() 
		{
			if (Application.isPlaying) 
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine (transform.position, m_groundCheck.transform.position);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (m_frontCheckStart, m_frontCheck.transform.position);
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
