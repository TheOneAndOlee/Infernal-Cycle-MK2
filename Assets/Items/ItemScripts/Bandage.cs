using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Bandage", menuName = "Items/Bandage")]
public class Bandage : BaseItem
{
    [SerializeField]
    private float regenMultiplier;

    [SerializeField]
    private float healInterval;

    public override IEnumerator PassiveItemEffect(ItemManager manager, int stacks)
    {
        while (true)
        {
            manager.Variables.Health += manager.Variables.MaxHealth * (regenMultiplier / 100) * stacks;

            yield return new WaitForSeconds(healInterval);
        }
    }
}
