using UnityEngine;

public class BossGunUpgrade : MonoBehaviour
{
    public void ApplyUpgrade()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        PlayerShoot ps = player.GetComponent<PlayerShoot>();
        if (ps != null)
        {
            ps.UpgradeGun();
            Debug.Log("🔫 Gun upgraded by this boss");
        }
    }
}
