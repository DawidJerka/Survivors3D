using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnCollisionStay(Collision collision)
    {
        PlayerDamageReceiver player =
            collision.collider.GetComponentInParent<PlayerDamageReceiver>();

        if (player == null)
            return;

        player.TryTakeDamage(damage);
    }
}