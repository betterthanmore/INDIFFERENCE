using UnityEngine;

public class DartSkill : Skill
{
    public GameObject dartPrefab; 
    public Transform firePoint;
    public float dartSpeed = 100f;

    public override void UseSkill()
    {
        if (IsCooldownComplete())
        {
            base.UseSkill();
            ThrowDart();
        }
    }

    private void ThrowDart()
    {
        GameObject dart = Instantiate(dartPrefab, firePoint.position, Quaternion.identity);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 
        Vector2 fireDirection = (mousePosition - firePoint.position).normalized;

        Rigidbody2D rb = dart.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = fireDirection * dartSpeed;
        }
    }
}