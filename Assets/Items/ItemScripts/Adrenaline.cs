using UnityEngine;

[CreateAssetMenu(fileName = "Adrenaline", menuName = "Items/Adrenaline")]
public class Adrenaline : BaseItem
{
    [Tooltip("The percentage increase in I-Frames")]
    [SerializeField] private float durationIncrease;

    public override void UseItem(ItemManager manager)
    {
        manager.Variables.IFrameDuration += manager.Variables.IFrameDuration * (durationIncrease / 100);
    }
}
