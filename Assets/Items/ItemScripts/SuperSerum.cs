using UnityEngine;

[CreateAssetMenu(fileName = "SuperSerum", menuName = "Items/Super Serum")]
public class SuperSerum : BaseItem
{
    [Tooltip("The item's decimal percentage damage boost")]
    [SerializeField] private float damageBoost;

    [Tooltip("The item's decimal percentage speed boost")]
    [SerializeField] private float speedBoost;

    [Tooltip("How much less damage the player takes as a decimal percentage")]
    [SerializeField] private float incomingDamageBuff;

    public override void UseItem(ItemManager manager)
    {
        manager.Variables.DamageMultiplier += manager.Variables.DamageMultiplier * (damageBoost / 100);
        manager.Variables.IncomingDamageMultiplier -= manager.Variables.IncomingDamageMultiplier * (incomingDamageBuff / 100);
        manager.Movement.MoveSpeed += manager.Movement.MoveSpeed * (speedBoost / 100);
    }
}
