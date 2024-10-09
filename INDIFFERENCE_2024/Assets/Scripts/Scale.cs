using UnityEngine;

public class Scale : MonoBehaviour
{
    public Transform leftPlatform;
    public Transform rightPlatform;
    public float sensitivity = 0.1f;
    public float smoothTime = 0.1f; 
    private float leftWeight;
    private float rightWeight;

    private Vector2 leftPlatformVelocity;
    private Vector2 rightPlatformVelocity;

    private void Update()
    {
        leftWeight = CalculateWeight(leftPlatform);
        rightWeight = CalculateWeight(rightPlatform);

        AdjustPlatforms();
    }

    private float CalculateWeight(Transform platform)
    {
        float weight = platform.GetComponent<Rigidbody2D>().mass;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(platform.position, 1f); 
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Object"))
            {
                weight += collider.GetComponent<Rigidbody2D>().mass;
            }
        }
        return weight;
    }

    private void AdjustPlatforms()
    {
        float heightDifference = (rightWeight - leftWeight) * sensitivity;

        float leftTargetY = leftPlatform.localPosition.y + heightDifference;
        float rightTargetY = rightPlatform.localPosition.y - heightDifference;

        leftPlatform.localPosition = new Vector2(leftPlatform.localPosition.x, Mathf.SmoothDamp(leftPlatform.localPosition.y, leftTargetY, ref leftPlatformVelocity.y, smoothTime));
        rightPlatform.localPosition = new Vector2(rightPlatform.localPosition.x, Mathf.SmoothDamp(rightPlatform.localPosition.y, rightTargetY, ref rightPlatformVelocity.y, smoothTime));

        leftPlatform.localPosition = new Vector2(leftPlatform.localPosition.x, Mathf.Clamp(leftPlatform.localPosition.y, -2f, 2f));
        rightPlatform.localPosition = new Vector2(rightPlatform.localPosition.x, Mathf.Clamp(rightPlatform.localPosition.y, -2f, 2f));
    }
}