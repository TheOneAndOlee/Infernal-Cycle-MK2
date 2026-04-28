using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Plea Deal", menuName = "Ability/Plea Deal")]
public class PleaDeal : BaseAbility
{
    [Tooltip("How much less damage the enemies do as a percent")]
    [SerializeField] private float damageReduction;

    public override void UseAbility(AbilityManager manager)
    {
        if (isOnCooldown)
        {
            Debug.Log("Plea Deal is on cooldown.");
            return;
        }

        base.UseAbility(manager);

        manager.StartCoroutine(PleaDealCoroutine());
    }

    private IEnumerator PleaDealCoroutine()
    {
        Debug.Log($"Plea Deal Activated! Incoming damage reduced by {damageReduction}% for {AbilityDuration} seconds.");
        
        if (PlayerVariables.Instance != null)
        {
            PlayerVariables.Instance.IncomingDamageMultiplier -= damageReduction * 100;
        }

        yield return new WaitForSeconds(AbilityDuration);

        if (PlayerVariables.Instance != null)
        {
            PlayerVariables.Instance.IncomingDamageMultiplier += damageReduction * 100;
        }

        Debug.Log("Plea deal expired.");
    }
}
