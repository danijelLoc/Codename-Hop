using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeReference] private Health protectorHealth;
    [SerializeReference] private Transform treasure;
    [SerializeField] private AudioClip soundOpen;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (protectorHealth.isDead())
            Invoke(nameof(Open), 1f);
    }

    private void Open()
    {
        if (soundOpen != null)
            SoundManager.instance.PlaySound(soundOpen);
        animator.SetTrigger("open");
    }

    public void ShowTreasure()
    {
        treasure.gameObject.SetActive(true);
    }
}
