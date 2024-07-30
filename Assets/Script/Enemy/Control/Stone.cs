using UnityEngine;

public class Stone : MonoBehaviour
{
    public float damage;
    public float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            DamageMessage damageMessage = new DamageMessage
            {
                amount = damage,
                damager = gameObject,
                hitPoint = collision.contacts[0].point,
                hitNormal = collision.contacts[0].normal
            };

            damageable.ApplyDamage(damageMessage);
        }

        Destroy(gameObject);
    }
}
