using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    private Text levelText;
    private GameObject levelImage;
    public bool doingSetup;
    public int pickDamage = 1;
    public int ammo = 0;
    public BoardManager boardScript;
    public static GameManager instance = null;
    public int level = 0;
    public int playerFoodPoints = 100;
    public float turnDelay = 0.1f;
    public List<Enemy> enemies;
    private bool enemiesMoving;
    private GameObject button;
    private GameObject selector;
    [HideInInspector] public bool playersTurn = true;

    

	void Awake () {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        AudioManager.instance.musicSource.Play();

    }


    void OnLevelFinishedLoading(Scene scene, LoadSceneMode
    mode)
    {

        level++;
        InitGame();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
  
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    public void GameOver()
    {
     
        levelText.text = "You made it to level " + level;
        levelImage.SetActive(true);
        button.SetActive(true);
        selector.SetActive(true);
        
        Destroy(gameObject);

    }
  

    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        button = GameObject.Find("LevelEndButtons");
        selector = GameObject.Find("Selector");
        selector.SetActive(false);
        button.SetActive(false);
        levelText.text = "Level " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
	// Update is called once per frame
	void Update () {
        if (playersTurn || enemiesMoving || doingSetup)
            return;
        StartCoroutine(MoveEnemies());
	}
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(turnDelay);

        
        if (enemies.Count == 0)
        {
            
            yield return new WaitForSeconds(turnDelay);
        }
        

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].hp <= 0)
            {

                Enemy toDelete = enemies[i];
                enemies.RemoveAt(i);
                toDelete.StartCoroutine(toDelete.Die());

                continue;
            }

            enemies[i].MoveEnemy();

            yield return new WaitForSeconds(enemies[i].moveTime);
        }
 
        playersTurn = true;
        
        enemiesMoving = false;
    }
    public void CheckEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].hp <= 0)
            {
                
                Enemy toDelete = enemies[i];
                toDelete.animator.SetTrigger("EnemyDamage");
                toDelete.animator.SetTrigger("EnemyDeath");
                enemies.RemoveAt(i);
                toDelete.StartCoroutine(toDelete.Die());

                continue;
            }
        }
     }
 
}
