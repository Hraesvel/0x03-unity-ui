using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    private Vector3 _direction;

    public Canvas hud;

    private int score = 0;
    public int health = 5;
    private float _startHealth;

    public Text scoreText;
    public Text healthText;

    private Image _healthTextBG;
    private Gradient _healthColor;

    public Color healthStart = Color.red;
    public Color healthEnd = Color.black;


    // Start is called before the first frame update
    void Start()
    {
        _healthColor = new Gradient();

        var colorkey = new GradientColorKey[2];

        colorkey[0].color = healthStart;
        colorkey[0].time = 1f;
        colorkey[1].color = healthEnd;
        colorkey[1].time = 0f;

        _healthColor.colorKeys = colorkey;
        _healthTextBG = healthText.transform.parent.GetComponent<Image>();


        scoreText.text = "Score: " + score;
        healthText.text = "Health: " + health;
        _healthTextBG.color = _healthColor.Evaluate(1);

        _startHealth = health;
    }

    private void Update()
    {
        if (health == 0)
        {
            Debug.Log("Game Over!");
            SceneManager.LoadScene("maze");
        }
    }

    private void FixedUpdate()
    {
        _direction.Set(0, 0, 0);

        if (UpdateDirection(ref _direction))
            GetComponent<Rigidbody>().AddForce((_direction * speed));
    }

    private static bool UpdateDirection(ref Vector3 dir)
    {
        if (!Input.anyKey) return false;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dir.x += 1;
        }


        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x -= 1;
        }


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dir.z += 1;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dir.z -= 1;
        }

        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            score += 1;
            SetScoreText();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Trap"))
        {
            health -= 1;
            SetHealthText(); 
           
            
        }

        if (other.CompareTag("Goal"))
        {
            Debug.Log("You win!");
        }
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score;
#if UNITY_EDITOR
        Debug.Log("Score: " + score);
#endif
    }

    void SetHealthText()
    {
        healthText.text = "Health: " + health;
        _healthTextBG.color = _healthColor.Evaluate(health / _startHealth);
#if UNITY_EDITOR
        Debug.Log("Health: " + health);
#endif
    }
}