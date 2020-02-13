using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird : MonoBehaviour
{
    public GameManager gm;
    public float velocity = 1;
    public int score;
    public GameObject scoreText;
    
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        score = 0;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * velocity;
        }

        scoreText.GetComponent<Text>().text = score.ToString();
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -0.69f, 1.22f), transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gm.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        score++;
    }
}
