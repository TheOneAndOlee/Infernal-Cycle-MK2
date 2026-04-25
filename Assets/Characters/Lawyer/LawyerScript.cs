using UnityEngine;
using UnityEngine.InputSystem;

public class LawyerScript : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Animator animator;
    [SerializeField] private MeleeWeaponScript meleeWeaponScript;
    public float EnemyDamage = 50f;
    
    private bool isCurrentlyAttacking = false;

    public void SetIsAttacking()
    {
        if (animator == null)
        {
            return;
        }

        isCurrentlyAttacking = true;
        animator.SetBool("IsAttacking", true);

        if (meleeWeaponScript != null)
        {
            meleeWeaponScript.ClearDamagedEnemies();
        }
    }

    public void SetIsNotAttacking()
    {
        if (animator == null)
        {
            return;
        }

        isCurrentlyAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    public void OnAttack(InputValue value)
    {
        if (animator == null || value == null)
        {
            return;
        }

        Debug.Log("IsCurrentlyAttacking: " + isCurrentlyAttacking + " | IsAttacking: " + animator.GetBool("IsAttacking"));

        if (isCurrentlyAttacking)
        {
            return;
        }

        if (value.isPressed)
        {
            SetIsAttacking();
        }
        
        Debug.Log("Using Melee Attack");
    }
}
