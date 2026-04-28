using UnityEngine;

[CreateAssetMenu(fileName = "HyperAmp", menuName = "Items/Hyper Amp")]

public class HyperAmp : BaseItem
{
    [Tooltip("The HyperAmp's percentage damage boost")]
    [SerializeField] private float damageBoost;

    [Tooltip("How much more damage the player takes as a percent")]
    [SerializeField] private float incomingDamageDebuff;

    public override void UseItem(ItemManager manager)
    {
        manager.Variables.DamageMultiplier += manager.Variables.DamageMultiplier * (damageBoost / 100);
        manager.Variables.IncomingDamageMultiplier += manager.Variables.IncomingDamageMultiplier * (incomingDamageDebuff / 100);
    }
}
