using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Detonator", menuName = "Items/Detonator")]

public class Detonator : BaseItem
{
    [Tooltip("Bonus damage done by this item")]
    [SerializeField] private float extraDamage;

    [SerializeField] private float cooldown;

    private float lastTriggered = -1f;

    private void OnEnable()
    {
        lastTriggered = -1f;
    }

    public override void OnEnemyDamaged(GameObject enemy, ItemManager manager)
    {
        if (Time.time >= lastTriggered + cooldown)
        {

        }
    }
}
