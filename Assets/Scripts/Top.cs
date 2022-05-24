using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Top : MonoBehaviour
{
    public GameObject GameOverView;
    public Text ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        GameOverView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Fruit"))
        {
            if((int)GameManager.gameManager.gameState < (int)GameState.GameOver)
            {
                if(collision.gameObject.GetComponent<Fruit>().fruitState == FruitState.Collision)
                {
                    GameManager.gameManager.gameState = GameState.GameOver;
                    Invoke(nameof(ShowGameOverView), 0.5f);
                    
                }
            }
        }
    }
    private void ShowGameOverView()
    {
        GameOverView.SetActive(true);
        ScoreText.text = "Score: " + GameManager.gameManager.score;
    }
    
}
