using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Selector : MonoBehaviour {
    int index = 0;
    public int totalItems = 2;
    public float yOffset = 40f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Scene current = SceneManager.GetActiveScene();
        string sceneName = current.name;
        if (sceneName == "Scene")
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (index < totalItems - 1)
                {
                    index++;
                    Vector2 position = transform.position;
                    position.y -= yOffset;
                    transform.position = position;
                }
                else
                {
                    index = 0;
                    Vector2 position = transform.position;
                    position.y += (totalItems - 1) * yOffset;
                    transform.position = position;
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (index > 0)
                {
                    index--;
                    Vector2 position = transform.position;
                    position.y += yOffset;
                    transform.position = position;
                }
                else
                {
                    index = totalItems - 1;
                    Vector2 position = transform.position;
                    position.y -= (totalItems - 1) * yOffset;
                    transform.position = position;
                }
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (index == 0)
                {
                    SceneManager.LoadScene(0);
                }
                if (index == 1)
                {
                    AudioManager.instance.musicSource.Play();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        } else if (sceneName == "menu")
        {
           
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (index == 0)
                {
                    
                    SceneManager.LoadScene(1);
              

                }
            }
        }
       


    }
  
    

}
