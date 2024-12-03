using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_CLICK);
        }
    }
}
