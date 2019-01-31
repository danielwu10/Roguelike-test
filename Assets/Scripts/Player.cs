using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

    public int wallDamage;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 0.5f;
    private Animator animator;
    private int food;
    public Text foodText;
    public Text pickText;
    public Text ammoText;
    public AudioClip shootSound;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip drinkSound1;
    public AudioClip ladder;
    public AudioClip gameOverSound;
    public AudioClip ammoSound;
    public AudioClip pickSound;
    private SpriteRenderer render;
    public Shoot gun;
    private string lastDirection;
    private bool move = false;
    public Vector3 facing;
    public float cooldown = 0.6f;
    private float nextFire = 0;
    private bool dead = false;
    // Use this for initialization
    protected override void Start () {
        render = GetComponent<SpriteRenderer>();
        gun = GetComponent<Shoot>();
        animator = GetComponent<Animator>();
        facing = new Vector3(0f, 0f, 90f); 
        food = GameManager.instance.playerFoodPoints;
        wallDamage = GameManager.instance.pickDamage;
        foodText.text = "Food: " + food;
        pickText.text = "" + wallDamage;
        ammoText.text = "" + gun.ammo;

        lastDirection = "Up";
        base.Start();

        

	}

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
        GameManager.instance.pickDamage = wallDamage;
        GameManager.instance.ammo = gun.ammo;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (!GameManager.instance.playersTurn || GameManager.instance.doingSetup) return;

        if (dead)
            return;

        int horizontal = 0;
        int vertical = 0;

        

 
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time > nextFire)
            {
               
                gun.ShootBullet();
                animator.SetTrigger("Shoot" + lastDirection);
                food--;
                foodText.text = "Food: " + food;
                nextFire = Time.time + cooldown;
                return;
               
            }

        }

        

        if (horizontal != 0)
            vertical = 0;
        
        if (horizontal != 0)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                facing = new Vector3(0f, 0f, 0f);

                if (lastDirection == "Right")
                {
                    move = true;
                }
                else
                {
                    lastDirection = "Right";
                }

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {

                facing = new Vector3(0f, 180f, 0f);

                if (lastDirection == "Left")
                {
                    move = true;
                }
                else
                {
                    lastDirection = "Left";
                }

            }
        } else if (vertical != 0)
        {
          
            if (Input.GetKey(KeyCode.UpArrow))

            {
                facing = new Vector3(0f, 0f, 90f);

                if (lastDirection == "Up")
                {
                    move = true;
                }
                else
                {
                    lastDirection = "Up";
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {


                facing = new Vector3(0f, 0f, -90f);

                if (lastDirection == "Down")
                {
                    move = true;
                }
                else
                {
                    lastDirection = "Down";
                }
            }
        }

  

        
        if (horizontal != 0 || vertical != 0)
        {
            if (move == false)
            {
                animator.SetTrigger(lastDirection + "Idle");
                horizontal = 0;
                vertical = 0;
                food--;
                foodText.text = "Food: " + food;
                CheckIfGameOver();
                GameManager.instance.playersTurn = false;
            } else
            {
                AttemptMove<Wall>(horizontal, vertical);
            }

            


        }

        move = false;
        return;

    }

    protected override void OnCantMove<T>(T component)
    {
        if (component == null )
        {
            return;
        }
        Wall hitWall = component as Wall;
        Debug.Log(hitWall.hp);
        if (hitWall.hp > 0)
        {
            animator.SetTrigger("Hit" + lastDirection);
            hitWall.DamageWall(wallDamage);
        }
      
     

    }
    private void Restart()
    {
        SceneManager.LoadScene(1);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            animator.SetTrigger("Up");
            AudioManager.instance.playClip(ladder);
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            AudioManager.instance.playClip(eatSound1);
            other.gameObject.SetActive(false);
            

        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            AudioManager.instance.playClip(drinkSound1);
            other.gameObject.SetActive(false);

        }
        else if (other.tag == "Pick")
        {
            wallDamage++;
            pickText.text = "" + wallDamage;
            AudioManager.instance.playClip(pickSound);
            other.gameObject.SetActive(false);

        }
        else if (other.tag == "Ammo")
        {
            gun.ammo++;
            ammoText.text = "" + gun.ammo;
            AudioManager.instance.playClip(ammoSound);
            other.gameObject.SetActive(false);

        }
    }
    public void LoseFood (int loss)
    {
        StartCoroutine(flash());
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food: " + food;
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        
        
        if (canMove)
        {
            base.AttemptMove<T>(xDir, yDir);
            animator.SetTrigger(lastDirection);
            AudioManager.instance.RandomizeSfx(moveSound1, moveSound2);
        } else
        {
            T wall = hit.transform.GetComponent<T>();
            OnCantMove(wall);
        }
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }

    private IEnumerator die()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(3);
        GameManager.instance.GameOver();
    }
    private IEnumerator flash()
    {
        render.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.3f);
        render.color = new Color(1, 1, 1);
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            
            AudioManager.instance.PlaySingle(gameOverSound);
            AudioManager.instance.musicSource.Stop();
            dead = true;
            StartCoroutine(die());
            
        }
    }


}
