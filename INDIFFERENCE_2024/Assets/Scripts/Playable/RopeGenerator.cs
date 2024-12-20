using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    public GameObject ropePrefab;
    public int ropeCount;
    public Rigidbody2D pointRb;
    private FixedJoint2D joint;
    public Vector2 anchorOffset = new Vector2(0, 1.5f);
    public float ropeMass = 1f;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < ropeCount; i++)
        {
            FixedJoint2D currentJoint = Instantiate(ropePrefab, transform).GetComponent<FixedJoint2D>();
            currentJoint.transform.localPosition = new Vector2(0, (i + 1) * -1f);
            currentJoint.anchor = anchorOffset;
            Rigidbody2D ropeRb = currentJoint.GetComponent<Rigidbody2D>();
            ropeRb.mass = ropeMass;
            if (i == 0)
            {
                currentJoint.connectedBody = pointRb;
            }
            else
            {
                currentJoint.connectedBody = joint.GetComponent<Rigidbody2D>();
            }
            joint = currentJoint;

            if(i == ropeCount - 1)
            {
                currentJoint.GetComponent<Rigidbody2D>().mass = 3;
            }
        }
        
    }
}
