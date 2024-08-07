using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    
    Rigidbody2D myRigidbody;
    PlayerMovement player;
    float xSpeed;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2 (xSpeed, 0);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);        
    }
}
