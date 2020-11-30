using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Camera cam;
	public GameObject ShotPrefab;
	public float playerSpeed = 3;
	public float shotSpeed = 1;
	public string combatType;

	private Rigidbody2D rb;
	private Transform tr;

	public Sprite knightSprite;
	public Sprite archerSprite;
	public Sprite mageSprite;
	public Sprite priestSprite;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		tr = GetComponent<Transform>();

		if(combatType == "Knight")
        {
			GetComponent<SpriteRenderer>().sprite = knightSprite;
			GetComponent<BoxCollider2D>().offset = new Vector2(0.3162517f, -0.100812f);
			GetComponent<BoxCollider2D>().size = new Vector2(0.9602444f, 2.918476f);
		} else if(combatType == "Archer")
        {
			//GetComponent<SpriteRenderer>().sprite = archerSprite;
        } else if(combatType == "Mage")
        {
			GetComponent<SpriteRenderer>().sprite = mageSprite;
			GetComponent<BoxCollider2D>().offset = new Vector2(-0.07492942f, 0.00812459f);
			GetComponent<BoxCollider2D>().size = new Vector2(1.049374f, 2.977896f);
		} else if(combatType == "Priest")
        {
			GetComponent<SpriteRenderer>().sprite = priestSprite;
        }
	}

	private void FixedUpdate()
	{
		Vector2 moveInput = new Vector2();
		if (Input.GetKey(KeyCode.D))
		{
			moveInput.x = playerSpeed;
		} else if(Input.GetKey(KeyCode.A))
		{
			moveInput.x = -playerSpeed;
		}

		if(Input.GetKey(KeyCode.W))
		{
			moveInput.y = playerSpeed;
		} else if(Input.GetKey(KeyCode.S))
		{
			moveInput.y = -playerSpeed;
		}

		rb.velocity = moveInput.normalized * playerSpeed;
	}

	// Update is called once per frame
	private void Update()
	{
		Vector3 mousePos = Input.mousePosition;
		if (combatType == "Archer")
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector2 shootVector = cam.ScreenToWorldPoint(mousePos) - tr.position;

				GameObject tempShot = Instantiate(ShotPrefab, tr.position, Quaternion.identity);

				tempShot.GetComponent<Rigidbody2D>().velocity = shootVector.normalized * shotSpeed;

				Destroy(tempShot, 20);
			}
		} else if(combatType == "Knight")
		{
			if (Input.GetMouseButtonDown(0))
			{
				Collider2D collider = Physics2D.OverlapCircle(cam.ScreenToWorldPoint(mousePos), 0);
				if (collider)
				{
					if(Vector2.Distance(collider.gameObject.transform.position, tr.position) < 4)
					{
						collider.gameObject.GetComponent<EnemyController>().loseHealth();
					}
				}
			}
		} else if(combatType == "Mage")
		{

		} else if(combatType == "Priest")
		{

		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			Destroy(collision.gameObject);

			Time.timeScale = 0;
		}
		//Debug.Log("Test");
	}
}
