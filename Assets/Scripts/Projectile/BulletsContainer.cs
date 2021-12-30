using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsContainer : MonoBehaviour
{
    [SerializeField] private int numberOfBullets;
    [SerializeReference] private Shooting playerShooting;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerShooting.AddBullets(numberOfBullets);
            gameObject.SetActive(false);
        }
    }
}
