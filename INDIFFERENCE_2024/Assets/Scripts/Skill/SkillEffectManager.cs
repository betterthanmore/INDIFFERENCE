using UnityEngine;
using System.Collections;

public class SkillEffectManager : MonoBehaviour
{
    public GameObject player;
    private MeshRenderer playerRenderer;
    private Collider2D playerCollider;
    private Rigidbody2D playerRig;
    private float transparencyDuration = 5f;

    private float originGravity;

    void Start()
    {
        playerCollider = player.GetComponent<Collider2D>();
        playerRig = player.GetComponent<Rigidbody2D>();
    }

    public void StartTransparency()
    {
        originGravity = playerRig.gravityScale;
        playerRenderer = player.GetComponent<MeshRenderer>();
        playerRenderer.material.color = new Color(1, 1, 1, 0.1f);
        playerRig.gravityScale = 0f;
        playerCollider.enabled = false;

        Debug.Log("투명화 스킬 발동: 벽 통과 가능");
        StartCoroutine(EndTransparency());
    }

    private IEnumerator EndTransparency()
    {
        yield return new WaitForSeconds(transparencyDuration);
        playerRenderer.material.color = new Color(1, 1, 1, 1f);
        playerRig.gravityScale = originGravity;
        playerCollider.enabled = true;

        Debug.Log("투명화 스킬 종료");
    }
}