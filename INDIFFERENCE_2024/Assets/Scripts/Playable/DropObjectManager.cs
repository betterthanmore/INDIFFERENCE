using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectManager : MonoBehaviour
{
    [System.Serializable]
    public class DropObject
    {
        public Transform objectTransform;
        public Transform triggerPosition;
        public float triggerDistance = 2.0f;
        public bool isFall = false;
    }

    public List<DropObject> dropObjects;
    public Transform player;

    private void Update()
    {
        foreach (DropObject dropObject in dropObjects)
        {
            if (dropObject.objectTransform == null || dropObject.triggerPosition == null)
            {
                continue;
            }

            float distance = Vector2.Distance(new Vector2(player.position.x, player.position.y), new Vector2(dropObject.triggerPosition.position.x, dropObject.triggerPosition.position.y));

            if (!dropObject.isFall && distance <= dropObject.triggerDistance)
            {
                FallObject(dropObject);
            }
        }
    }

    void FallObject(DropObject fallingObject)
    {
        Rigidbody2D rb = fallingObject.objectTransform.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false; 
        }

        fallingObject.isFall = true;
    }
}
