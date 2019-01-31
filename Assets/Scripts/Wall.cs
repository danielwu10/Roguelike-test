using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public int hp;
    private int maxhp;
    public Sprite dmgSprite;
    public Sprite dmgSprite2;
    public Sprite destroyed1;
    public Sprite destroyed2;
    private SpriteRenderer spriteRenderer;
    private int level;
    private BoxCollider2D box;
    public AudioClip chopSound1;
    public GameObject particle;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        level = GameManager.instance.level;
        hp = (int)Mathf.Log(level, 2f);
        if (hp == 0)
        {
            hp = 1;
        }
        //Get a component reference to the SpriteRenderer.
        maxhp = hp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }


    private IEnumerator broke()
    {
        spriteRenderer.sprite = destroyed1;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = destroyed2;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = destroyed1;
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
    public void DamageWall(int loss)
    {
        Instantiate(particle, transform.position, transform.rotation);
        if (hp > (maxhp / 2))
        {
            spriteRenderer.sprite = dmgSprite;
        } else
        {
            
            spriteRenderer.sprite = dmgSprite2;
        }
       
        AudioManager.instance.RandomizeSfx(chopSound1);

        hp -= loss;

        if (hp <= 0)
        {
            box.enabled = false;
            StartCoroutine(broke());
        }
        
            
    }
  
}
