using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ready = 1,
    InProgress = 2,
    GameOver = 3,
}
public class GameManager : MonoBehaviour
{
    public GameObject[] FruitList;
    public GameObject BornPos;
    public GameObject StartButton;
    public GameState gameState;
    public float score = 0f;
    public Text scoreText;
    public AudioSource CombineAudio;
    public static GameManager gameManager;
    private Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
    // Start is called before the first frame update
    void Start()
    {
        gameManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == GameState.GameOver)
        {
          
            if (Input.GetMouseButton(0))
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }
    public void StartGame()
    {
        StartButton.SetActive(false);
        gameState = GameState.Ready;
        CreateFruit();
        
    }
    // randomly generate a fruit from grape to kiwifruit at default position
    public void CreateFruit()
    {       
        int num = Random.Range(0, 5);
        GameObject fruit = FruitList[num];
        var curFruit = Instantiate(fruit, BornPos.transform.position, fruit.transform.rotation);
        curFruit.GetComponent<Fruit>().fruitState = FruitState.Ready;
    }
    public void InvokeCreateFruit()
    {
        //create a fruit after a delay of 0.5 second
        Invoke(nameof(CreateFruit), 1f);
    }

    //calculate the center position of two old fruits, and create a higher-level one at that position
    public void CombineFruit(Vector3 currentPos, Vector3 collisionPos, FruitType currentType)
    {       
        Vector3 newPos = (currentPos + collisionPos) / 2;
        //Debug.Log("cur: " + currentPos + ", coli: " + collisionPos + " new: " + newPos);
        GameObject newFruit = FruitList[(int)currentType];
        var combineFruit = Instantiate(newFruit, newPos, newFruit.transform.rotation);
        combineFruit.GetComponent<Fruit>().fruitState = FruitState.Collision;
        combineFruit.GetComponent<Rigidbody2D>().gravityScale = 1f;       
        combineFruit.transform.localScale = scale;
        scoreText.text = "Score: " + score;
        CombineAudio.Play();
    }
}
