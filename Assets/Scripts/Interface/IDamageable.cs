public interface IDamageable
{
    bool ApplyDamage(DamageMessage damageMessage);
    void Die();
}
