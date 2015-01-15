using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class PlayerController : MonoBehaviour 
	{
		public float moveSpeed = 8f;
		public float minSpeed = 2f;
		public float jumpHeight = 2f;
		public float deathLevel = -1;
	
		private bool m_bJump = false;
		private bool m_bDoubleJump = false;
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
		private int playerAnimAttack = Animator.StringToHash("playerAnimAttack");
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

			if (transform.position.y < deathLevel) 
			{
				m_gameLogic.SetDead();
			}

			// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
			//m_bGrounded = Physics2D.Linecast(transform.position, m_groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

			m_bGrounded = false;
			RaycastHit2D groundHit = Physics2D.CircleCast(transform.position, 0.5f, -Vector2.up, 0.6f, LayerMask.GetMask ("Ground"));
			if (groundHit) 
			{
				//Debug.Log("groundhit: " + groundHit.collider.name);
				m_bGrounded = true;
				m_animator.SetBool (playerAnimJump, false);
			}
		

			// Check front collision
			//m_frontCheckStart.x = transform.position.x;
			//m_frontCheckStart.y = m_frontCheck.position.y;
			//m_bFrontCollision = Physics2D.Linecast(m_frontCheckStart, m_frontCheck.position, 1 << LayerMask.NameToLayer("Ground"));

			m_bFrontCollision = false;
			RaycastHit2D wallHit = Physics2D.CircleCast(transform.position, 0.8f, Vector2.right, 0.2f, LayerMask.GetMask("Ground"));
			if(wallHit)
			{
				//Debug.Log("wallhit: " + wallHit.collider.name);
				m_bFrontCollision = true;
			}

			if (m_bGrounded)
			{
				if (useAnimations) 
				{
					m_animator.SetBool(playerAnimJump, false);
				}
			}
			m_animator.SetBool(playerAnimAttack, false);
			if (Input.GetKeyDown (KeyCode .A)) 
			{
				m_animator.SetBool(playerAnimAttack, true);
			}

			if(Input.GetButtonDown ("Jump") && (m_gameStateController.GameState == State.LEVELRUNNING) )
			{
				Debug.Log("jump!!");
				if (useAnimations) 
				{	
					m_animator.SetBool(playerAnimJump, true);
				}
				if(m_bGrounded)
				{
					m_bJump = true;
				}
				else if(m_gameLogic.Peasoup > 0)
				{
					Debug.Log("doubleJump!!");
					m_gameLogic.RemovePeasoup();
					m_bJump = true;
				}
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

				if(rigidbody2D.velocity.magnitude > moveSpeed)
				{
					Vector2 currentVelocity = rigidbody2D.velocity;
					currentVelocity = rigidbody2D.velocity.normalized * moveSpeed;
					rigidbody2D.velocity = currentVelocity;
				}
			} 

			if(m_bJump)
			{
				Debug.Log ("jump velocity add!");

				float jumpForce = Mathf.Sqrt (2.0f * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
				Vector2 currentVelocity = rigidbody2D.velocity;
				currentVelocity.y = jumpForce;
				rigidbody2D.velocity = currentVelocity;

				m_bJump = false;
			}

			/*
			if(rigidbody2D.velocity.x < 0.001f && !m_bGrounded)
			{
				Debug.Log ("adding up force");
				rigidbody2D.AddForce (Vector2.up * 10f, ForceMode2D.Force);
			}*/

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

				Vector3 s = transform.position;
				s.y = s.y - 0.6f;
				Gizmos.DrawWireSphere(s, 0.5f);

				Gizmos.color = Color.blue;
				Vector3 p = transform.position;
				p.x = p.x + 0.2f;
				Gizmos.DrawWireSphere(p, 0.8f);
			}
		}

		void OnCollisionEnter2D(Collision2D coll) 
		{
			switch(coll.collider.tag)
			{
			case "Enemy":
				rigidbody2D.AddForce(-Vector2.right * 10f, ForceMode2D.Impulse);
				break;
			default:
				break;
			}
		}

		public void ChangeMaxSpeed(float speedChange)
		{
			moveSpeed = moveSpeed + speedChange;
		}
	}
}
