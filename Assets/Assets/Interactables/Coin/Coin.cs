using System;
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !wasCollected)
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        wasCollected = true;
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddCoins(1);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
