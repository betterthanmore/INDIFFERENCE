using UnityEngine;
using UnityEngine.U2D.IK;

public class IKController : MonoBehaviour
{
    public Transform leftFoot;  
    public Transform rightFoot; 
    public LayerMask groundLayer; 
    public float footOffset = 0.1f; 

    private void Update()
    {
        AdjustFootPosition(leftFoot);
        AdjustFootPosition(rightFoot);
    }

    private void AdjustFootPosition(Transform foot)
    {
        RaycastHit2D hit = Physics2D.Raycast(foot.position + Vector3.up, Vector2.down, 1f, groundLayer);
        if (hit.collider != null)
        {
            foot.position = new Vector3(foot.position.x, hit.point.y + footOffset, foot.position.z);
        }
    }
}
