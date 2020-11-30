using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private Seeker seeker;

    public int life;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        seeker = GetComponent<Seeker>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector2.Distance(this.transform.position, player.transform.position) < 10)
        {
            seeker.StartPath(transform.position, player.transform.position);
        } else
        {
            seeker.StartPath(transform.position, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Shot")
        {
            Destroy(collision.gameObject);

            loseHealth();
        }
    }

    public void loseHealth()
    {
        life -= 1;

        if (life == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
