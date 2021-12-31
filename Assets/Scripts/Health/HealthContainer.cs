using UnityEngine;

public class HealthContainer : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip sound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<Health>().currentHealth == collision.GetComponent<Health>().getStartingHealth()) return;
            collision.GetComponent<Health>().AddHealth(healthValue);
            SoundManager.instance.PlaySound(sound);
            gameObject.SetActive(false);
        }
    }
}