using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeReference] private Health protectorHealth;
    private Animator animator;
    private bool isOpen = false;

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
        animator.SetTrigger("open");
        isOpen = true;
    } 
}
