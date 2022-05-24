using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Eleven = 11,
}
public enum FruitState
{
    Ready = 1,
    Dropping = 2,
    Collision = 3,
}
public class Fruit : MonoBehaviour
{
    public FruitType fruitType;
    public FruitState fruitState;
    public float fruitScore;
    public float limit_x;
    private Vector3 defaultScale = new Vector3(0.6f, 0.6f,0.6f);

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameManager.gameState == GameState.Ready && fruitState == FruitState.Ready)
        {
            //before dropping set fruit following mouse
            if(fruitState == FruitState.Ready)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.gameObject.GetComponent<Transform>().position = new Vector3(Mathf.Clamp(mousePos.x, -limit_x, limit_x),
                     this.gameObject.GetComponent<Transform>().position.y, this.gameObject.GetComponent<Transform>().position.z);
            }
            
            
            //click mouse and the fruit drops
            if (Input.GetMouseButtonUp(0))
            {
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
                fruitState = FruitState.Dropping;
                GameManager.gameManager.gameState = GameState.InProgress;
                GameManager.gameManager.InvokeCreateFruit();
            }
        }

        //simulate gradient animation when new fruit combined
        if(this.transform.localScale.x < defaultScale.x)
        {
            this.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        }
        else
        {
            this.transform.localScale = defaultScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Wall") || collision.gameObject.tag.Contains("Fruit"))
        {
            fruitState = FruitState.Collision;
            GameManager.gameManager.gameState = GameState.Ready;
            
        }
        
        //when two same fruits collide, call CombineFruit function to get a new one and destroy these two 
        if ((int)fruitState >= (int)FruitState.Dropping && collision.gameObject.tag.Contains("Fruit"))
        {                       
             if(fruitType == collision.gameObject.GetComponent<Fruit>().fruitType && fruitType!=FruitType.Eleven)
             {
                 if(this.gameObject.GetInstanceID() > collision.gameObject.GetInstanceID())
                 {
                    GameManager.gameManager.score += fruitScore;
                    GameManager.gameManager.CombineFruit(this.gameObject.GetComponent<Transform>().position, collision.transform.position, fruitType);                   
                    Destroy(collision.gameObject);
                    Destroy(this.gameObject);
                 }
                    
             }
            
        }
    }
}
