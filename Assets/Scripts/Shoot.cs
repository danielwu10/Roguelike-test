using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour {

    public Transform firePoint;
    public Transform firePoint2;
    public GameObject bulletPrefab;
    public Text ammoText;
    public int ammo;
    public Vector3 facing;
    public AudioClip shootSound;
    public AudioClip emptySound;


    private void Awake()
    {
        ammo = GameManager.instance.ammo;
        ammoText = GetComponentInParent<Player>().ammoText;
       
    }

    // Update is called once per frame
    void Update () {
        
	
	}
    public void ShootBullet()
    {
        facing = GetComponentInParent<Player>().facing;
        firePoint2 = firePoint;
        if (ammo == 0)
        {
            AudioManager.instance.playClip(emptySound);
            GameManager.instance.playersTurn = false;
            return;
        }
        AudioManager.instance.playClip(shootSound);
        if (facing.z == -90f)
        {
            
            firePoint.Translate(-0.3f, 0, 0);
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(facing));
            firePoint.Translate(0.3f, 0, 0);
        } else
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(facing));
        }
        ammo--;
        ammoText.text = "" + ammo;


      

        

        
    }
}
