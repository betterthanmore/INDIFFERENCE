using UnityEngine;

public class DartSkill : Skill
{
    public GameObject dartPrefab; 
    public Transform firePoint;
    private float dartSpeed = 50f;
    public PlayerController player;

    public override void UseSkill()
    {
        if (IsCooldownComplete())
        {
            base.UseSkill();
            ThrowDart();
            player.Attack();
        }
    }

    private void ThrowDart()
    {
        GameObject dart = Instantiate(dartPrefab, firePoint.position, Quaternion.identity);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 
        Vector2 fireDirection = (mousePosition - firePoint.position).normalized;
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;

        dart.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Rigidbody2D rb = dart.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = fireDirection * dartSpeed;
        }
    }
}