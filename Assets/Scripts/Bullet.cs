using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject particle;
    public AudioClip hitSound;

	// Use this for initialization
	void Start () {
        rb.velocity = transform.right * speed;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.tag == "Wall")
        {
            AudioManager.instance.playClip(hitSound);
            Instantiate(particle, transform.position, transform.rotation);
            Destroy(gameObject);
            GameManager.instance.playersTurn = false;


        }
        else if (other.tag == "Enemy")
        {
           
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.loseHp();
            }
            GameManager.instance.CheckEnemy();
            Destroy(gameObject);
            GameManager.instance.playersTurn = false;
        }
        
        
    }
}
