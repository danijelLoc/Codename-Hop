using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalContainer : MonoBehaviour
{
    [SerializeReference] private Health protectorHealth;
    [SerializeReference] private Transform portal;


    // Update is called once per frame
    void Update()
    {
        if (protectorHealth.isDead())
            Invoke(nameof(OpenPortal), 1f);
    }

    private void OpenPortal()
    {
        portal.gameObject.SetActive(true);
    }
}
