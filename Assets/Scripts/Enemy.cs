using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int playerDamage;
    public Animator animator;
    private Transform target;
    private bool skipMove;
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;
    public AudioClip enemyHurt;
    private BoxCollider2D collision;
    public GameObject particle;
    public int hp = 2;


    // Use this for initialization
    protected override void Start () {
        GameManager.instance.AddEnemyToList(this);
        collision = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();

	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void MoveEnemy()
    {
       
        int xDir = 0;
        int yDir = 0;
        
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)


            yDir = target.position.y > transform.position.y ? 1 : -1;

    
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

       
        AttemptMove<Player>(xDir, yDir);
    }
    protected override void OnCantMove<T>(T component)
    {
       
        Player hitPlayer = component as Player;
        float x = hitPlayer.transform.position.x;
        float y = hitPlayer.transform.position.y;
        if (x+1 == transform.position.x)
        {
            animator.SetTrigger("EnemyLeft");
        }
        else if (x -1  == transform.position.x) {
            animator.SetTrigger("EnemyRight");
        }
        else if (y - 1 == transform.position.y)
        {
            animator.SetTrigger("EnemyUp");
        }
        else if (y + 1 == transform.position.y)
        {
            animator.SetTrigger("EnemyDown");
        }

       
        hitPlayer.LoseFood(playerDamage);
        
        AudioManager.instance.RandomizeSfx(enemyAttack1);
        

    }
    public void loseHp()
    {
        Instantiate(particle, transform.position, transform.rotation);
        AudioManager.instance.playClip(enemyHurt);
        animator.SetTrigger("EnemyDamage");
        hp--;
    }

    public IEnumerator Die()
    {
        
       
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
