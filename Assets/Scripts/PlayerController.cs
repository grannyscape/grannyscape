using UnityEngine;
using System.Collections;

namespace grannyscape
{

	public class PlayerController : MonoBehaviour 
	{
		public float hitDistance = 1;
		public float moveSpeed = 8f;
		public float minSpeed = 2f;
		public float jumpHeight = 2f;
		public float deathLevel = -1;
	
		private bool m_bJump = false;
		private bool m_bGrounded = false;
		private bool m_bFrontCollision = false;

		private Animator m_animator;

		private GameStateController m_gameStateController;
		private GameLogic m_gameLogic;
		private PersistentData m_persistentData;
		private AudioManager m_audioManager;

		// hash the animation state string to save performance
		private int playerAnimJump =  Animator.StringToHash("playerAnimJump");
		private int playerAnimMove = Animator.StringToHash("playerAnimMove");
		private int playerAnimAttack = Animator.StringToHash("playerAnimAttack");
		private int playerAnimWin = Animator.StringToHash("playerAnimWin");
		private int playerAnimIdle = Animator.StringToHash("playerAnimIdle");

		public GameObject fartPrefab;
		public AudioClip[] farts;

		void Awake()
		{
		}

		// Use this for initialization
		void Start () 
		{
			GameObject sceneEssentials = GameObject.Find("SceneEssentials");
			m_gameStateController = sceneEssentials.GetComponent<GameStateController>();
			m_gameLogic = sceneEssentials.GetComponent<GameLogic>();
			m_audioManager = sceneEssentials.GetComponent<AudioManager>();

			m_persistentData = GameObject.Find ("PersistentData").GetComponent<PersistentData>();

			m_animator = GetComponentInChildren<Animator> ();
			m_animator.SetBool (playerAnimJump, false);
			m_animator.SetFloat (playerAnimMove, 0.0f);
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (m_gameStateController.GameState == State.LEVELEND) 
			{
				m_animator.SetBool(playerAnimWin, true);
			}

			if (m_gameStateController.GameState != State.LEVELRUNNING) 
			{
				return;
			}

			if (transform.position.y < deathLevel) 
			{
				m_gameLogic.SetDead(DeathReason.KICKET_THE_BUCKET);
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
				m_animator.SetBool(playerAnimJump, false);
			}
			Debug.DrawRay (transform.position, Vector2.right * hitDistance, Color.red);


			AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
			bool isAttacking = info.IsName ("attackShort");
			m_animator.SetBool(playerAnimAttack, false);
			if (!isAttacking && Input.GetKeyDown (KeyCode.A)) 
			{
				m_audioManager.PlaySound(SoundType.GRANNY_STICK_SWING);
				m_animator.SetBool(playerAnimAttack, true);

				/*RaycastHit2D enemyHit = Physics2D.CircleCast(transform.position, 0.8f, Vector2.right, hitDistance, LayerMask.GetMask("Enemy"));
				if(enemyHit)
				{
					//Debug.Log (enemyHit.transform.name);
					//Debug.Log (enemyHit.transform.tag);
					enemyHit.transform.gameObject.GetComponent<EnemyController>().Hit();
				}*/
			}

			if(Input.GetButtonDown ("Jump") && (m_gameStateController.GameState == State.LEVELRUNNING) )
			{
				m_animator.SetBool(playerAnimJump, true);

				if(m_bGrounded)
				{
					m_bJump = true;
				}
				else if(m_persistentData.Peasoup > 0)
				{
					m_audioManager.PlaySound(SoundType.GRANNY_FART);
					Instantiate (fartPrefab, transform.position, Quaternion.identity);
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
				//float jumpRatio = rigidbody2D.velocity.x / moveSpeed;
				//Mathf.Clamp(jumpRatio, 0.5f, 1f);

				float jumpForce = Mathf.Sqrt (2.0f * jumpHeight * Mathf.Abs(Physics2D.gravity.y)); // * jumpRatio;
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


			m_animator.SetBool (playerAnimIdle, rigidbody2D.velocity.x == 0.0f);
			m_animator.SetFloat (playerAnimMove, rigidbody2D.velocity.x);

			if (rigidbody2D.velocity.x < 0.01f && rigidbody2D.velocity.x > -0.01f) {
				Vector2 v0 = rigidbody2D.velocity;
				v0.x = 0.0f;
				rigidbody2D.velocity = v0;
			}
		}

		void OnDrawGizmos() 
		{
			/*
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

				Gizmos.color = Color.red;
				Vector3 z = transform.position;
				z.x = z.x + 1f;
				Gizmos.DrawWireSphere(z, 0.8f);
			}
			*/
		}

		public void OnActionCainHit()
		{
			RaycastHit2D enemyHit = Physics2D.CircleCast(transform.position, 0.8f, Vector2.right, hitDistance, LayerMask.GetMask("Enemy"));
			if(enemyHit)
			{
				//Debug.Log (enemyHit.transform.name);
				//Debug.Log (enemyHit.transform.tag);
				enemyHit.transform.gameObject.GetComponent<EnemyController>().Hit();
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

		public void StopMovement()
		{
			rigidbody2D.velocity = Vector2.zero;
		}
	}
}
