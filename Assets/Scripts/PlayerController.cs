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

		public GameObject fartPrefab;
		public AudioClip[] farts;

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
				m_animator.SetFloat(playerAnimMove, 0.0f);
				return;
			}

			if (transform.position.y < deathLevel) 
			{
				m_gameLogic.SetDead();
			}

			// Check ground collision
			m_bGrounded = false;
			RaycastHit2D groundHit = Physics2D.CircleCast(transform.position, 0.5f, -Vector2.up, 0.6f, LayerMask.GetMask ("Ground"));
			if (groundHit) 
			{
				m_bGrounded = true;
				m_animator.SetBool (playerAnimJump, false);
			}

			// Check front collision
			m_bFrontCollision = false;
			RaycastHit2D wallHit = Physics2D.CircleCast(transform.position, 0.8f, Vector2.right, 0.2f, LayerMask.GetMask("Ground"));
			if(wallHit)
			{
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
					if (farts.Length > 0) 
					{
						AudioSource.PlayClipAtPoint(farts [Random.Range (0, farts.Length - 1)], Camera.main.transform.position);
					}
					Instantiate (fartPrefab, transform.position, Quaternion.identity);

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
				//TODO: testaa
				float jumpRatio = rigidbody2D.velocity.x / moveSpeed;
				Mathf.Clamp(jumpRatio, 0.5f, 1f);

				float jumpForce = Mathf.Sqrt (2.0f * jumpHeight * Mathf.Abs(Physics2D.gravity.y)) * jumpRatio;
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
