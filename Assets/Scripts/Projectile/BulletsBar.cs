using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsBar : MonoBehaviour
{
    [SerializeReference] Text bulletsCountText;
    [SerializeReference] Shooting shooting;

    // Update is called once per frame
    void Update()
    {
        bulletsCountText.text = shooting.numberOfBullets.ToString();
    }
}
