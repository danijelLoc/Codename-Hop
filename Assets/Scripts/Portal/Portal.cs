using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PauseMenu.LevelComplete = true;
            collision.gameObject.SetActive(false);
            Invoke(nameof(Hide), 1f);
        }
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
