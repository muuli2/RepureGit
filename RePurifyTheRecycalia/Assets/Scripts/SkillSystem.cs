using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SkillSystem : MonoBehaviour
{
    public Key skillKey = Key.E;
    public float cooldown = 10f;
    private float lastUsedTime = -Mathf.Infinity;

    [Header("UI")]
    public Image skillIcon;
    public Image cooldownOverlay;

    [Header("Effect")]
    public GameObject skillEffectPrefab; // กระสุน/เอฟเฟกต์สกิล
    public Transform firePoint;

    void Update()
    {
        float timeLeft = (lastUsedTime + cooldown) - Time.time;

        // อัปเดต UI Overlay
        if (cooldownOverlay != null)
            cooldownOverlay.fillAmount = Mathf.Clamp01(timeLeft / cooldown);

        // ใช้สกิล E
        if (Keyboard.current[skillKey].wasPressedThisFrame && timeLeft <= 0)
            UseSkill();
    }

    void UseSkill()
    {
        lastUsedTime = Time.time;

        if(skillEffectPrefab != null && firePoint != null)
            Instantiate(skillEffectPrefab, firePoint.position, firePoint.rotation);

        Debug.Log("ใช้สกิล E!");
    }
}
