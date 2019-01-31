using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;
        
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    public int columns = 8;
    public int rows = 8;
    public int level;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] upgradeTiles;
    public GameObject[] ammoTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;

    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if ((x == 0)&&(y == 0))
                {
                    continue;
                }
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }

    }

    void BoardSetup()
    {
        
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        
        int randomIndex = Random.Range(0, gridPositions.Count);

        Vector3 randomPosition = gridPositions[randomIndex];

        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }


    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();

            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    void LayoutChance(GameObject[] tileArray, int chance,int max)
    {
        

        for (int i = 0; i < max; i++)
        {
            int rand = Random.Range(0, 100);

            if (rand <= chance)
            {
                Vector3 randomPosition = RandomPosition();

                GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

                Instantiate(tileChoice, randomPosition, Quaternion.identity);
            }


        }
    }

    void LayoutWalls(int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            int rand = Random.Range(0, 100);
            Vector3 randomPosition = RandomPosition();

            GameObject tileChoice = wallTiles[Random.Range(0, wallTiles.Length)];

            Instantiate(tileChoice, randomPosition, Quaternion.identity);
            if (rand <= 5)
            {
                Instantiate(upgradeTiles[0], randomPosition, Quaternion.identity);
            } else if (rand <= 10)
            {
                Instantiate(ammoTiles[0], randomPosition, Quaternion.identity);
            } else if (rand <= 30)
            {
                GameObject tileChoice2 = foodTiles[Random.Range(0, foodTiles.Length)];
                Instantiate(tileChoice2, randomPosition, Quaternion.identity);
            }
            
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();

        InitialiseList();
        level = GameManager.instance.level;

        int rand = Random.Range(0, 8);
        Vector3 ladder = new Vector3(rand, rows - 1, 0f);
        Instantiate(exit, ladder , Quaternion.identity);
        gridPositions.Remove(ladder);
 

        LayoutWalls(wallCount.minimum, wallCount.maximum);

        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        LayoutChance(ammoTiles, 50, 3);

        LayoutChance(upgradeTiles, 30, 1);

        int enemyCount = (int)Mathf.Log(level, 2f);

        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        
    }
   
 
}
