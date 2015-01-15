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
		private bool m_bFrontCollision = false;

		private Transform m_groundCheck;
		private Transform m_frontCheck;
		private Vector2 m_frontCheckStart = new Vector2(0f, 0f);

		private Animator m_animator;

		private GameStateController m_gameStateController;
		private GameLogic m_gameLogic;

		// hash the animation state string to save performance
		private int playerAnimJump =  Animator.StringToHash("playerAnimJump");
		private int playerAnimMove = Animator.StringToHash("playerAnimMove");

		public bool useAnimations = false;

		void Awake()
		{
			m_groundCheck = transform.Find("groundCheck");
			m_frontCheck = transform.Find("frontCheck");
		}

		// Use this for initialization
		void Start () 
		{
			GameObject sceneEssentials = GameObject.Find("SceneEssentials");
			m_gameStateController = sceneEssentials.GetComponent<GameStateController>();
			m_gameLogic = sceneEssentials.GetComponent<GameLogic>();

			if (useAnimations) 
			{
				m_animator = GetComponentInChildren<Animator> ();
				m_animator.SetBool (playerAnimJump, false);
				m_animator.SetFloat (playerAnimMove, 0.0f);
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (m_gameStateController.GameState != State.LEVELRUNNING) 
			{
				return;
			}

			// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
			m_bGrounded = Physics2D.Linecast(transform.position, m_groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

			// Check front collision
			m_frontCheckStart.x = transform.position.x;
			m_frontCheckStart.y = m_frontCheck.position.y;
			//m_bFrontCollision = Physics2D.Linecast(m_frontCheckStart, m_frontCheck.position, 1 << LayerMask.NameToLayer("Ground"));

			RaycastHit2D hit = Physics2D.CircleCast(m_frontCheck.position, 0.8f, Vector2.right);
			if(hit && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
			{
				//Debug.Log("hit: " + hit.collider.name);
				m_bFrontCollision = true;
			}
			else
			{
				m_bFrontCollision = false;
			}


			if (m_bGrounded)
			{
				if (useAnimations) 
				{
					m_animator.SetBool(playerAnimJump, false);
				}
			}

			
			if(Input.GetButtonDown ("Jump") && m_bGrounded && (m_gameStateController.GameState == State.LEVELRUNNING) )
			{
				if (useAnimations) 
				{	
					m_animator.SetBool(playerAnimJump, true);
				}
				m_bJump = true;
			}
		}

		void FixedUpdate () 
		{
			if (m_gameStateController.GameState != State.LEVELRUNNING) 
			{
				return;
			}

			if (!m_bFrontCollision)
			{
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
				float jumpRatio = rigidbody2D.velocity.magnitude / maxSpeed;
				Mathf.Clamp(jumpRatio, 0.5f, 1f);

				float jumpForce = Mathf.Sqrt (2.0f * jumpHeight * Mathf.Abs(Physics2D.gravity.y)) * jumpRatio;
				Vector2 currentVelocity = rigidbody2D.velocity;
				currentVelocity.y = jumpForce;
				rigidbody2D.velocity = currentVelocity;

				m_bJump = false;

			}

			if(rigidbody2D.velocity.x < 0.001f && !m_bFrontCollision && !m_bGrounded)
			{
				rigidbody2D.AddForce (Vector2.up * 10);
			}

			if (useAnimations) 
			{
				m_animator.SetFloat (playerAnimMove, rigidbody2D.velocity.x);
			}
		}

		void OnDrawGizmos() 
		{
			if (Application.isPlaying) 
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine (transform.position, m_groundCheck.transform.position);
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(m_frontCheck.position, 0.8f);

			}
		}

	}
}
