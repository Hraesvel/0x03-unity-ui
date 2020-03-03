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
    
    private Text _scoreText;
    private Text _healthText;
    private Gradient _healthColor;

    public Color healthStart = Color.green;
    public Color healthEnd = Color.red;


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



        _healthText = hud.transform.Find("Health").GetComponent<UnityEngine.UI.Text>();
        _scoreText = hud.transform.Find("Score").GetComponent<UnityEngine.UI.Text>();

        _scoreText.text = "Score: " + score;
        _healthText.text = "Health: " + health;
        _healthText.color = _healthColor.Evaluate(1);

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
            _scoreText.text = "Score: " + score;
            Debug.Log("Score: " + score);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Trap"))
        {
            health -= 1;
            _healthText.text = "Health: " + health;
            _healthText.color = _healthColor.Evaluate(health / _startHealth);
            Debug.Log("Health: " + health);
        }

        if (other.CompareTag("Goal"))
        {
            Debug.Log("You win!");
        }

    }

}