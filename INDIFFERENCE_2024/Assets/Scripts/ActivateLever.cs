using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLever : MonoBehaviour, IInteractable
{
    public List<GameObject> movingPlatformer;
    public GameObject Lever;

    private bool isActivated = false;

    public void Interact()
    {
        float targetAngle = isActivated ? 45f : -45f;
        Lever.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);

        isActivated = !isActivated;

        foreach(GameObject platform in movingPlatformer)
        {
            MovingPlatform platformScript = platform.GetComponent<MovingPlatform>();
            if(platform != null)
            {
                platformScript.enabled = !platformScript.enabled;
            }
        }
    }
}
