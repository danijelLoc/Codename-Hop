using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniHealthBar : MonoBehaviour
{
    [SerializeReference] Health health;
    private float initialScaleX;

    void Awake()
    {
        initialScaleX = transform.localScale.x;
    }

    void Update()
    {
        float currentScaleX = initialScaleX * health.currentHealth / health.getStartingHealth();
        transform.localScale = new Vector3(currentScaleX, transform.localScale.y, transform.localScale.z);
    }
}
