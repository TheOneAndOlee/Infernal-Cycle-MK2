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
            manager.Player.Health += manager.Player.MaxHealth * regenMultiplier * stacks;

            yield return new WaitForSeconds(healInterval);
        }
    }
}
