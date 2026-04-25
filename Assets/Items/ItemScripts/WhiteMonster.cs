using UnityEngine;

[CreateAssetMenu(fileName = "WhiteMonster", menuName = "Items/White Monster")]
public class WhiteMonster : BaseItem
{
    [SerializeField] private float speedBoost;

    public override void UseItem(ItemManager manager)
    {
        manager.PlayerMovement.MoveSpeed += manager.PlayerMovement.DefaultMoveSpeed * speedBoost;
    }
}
