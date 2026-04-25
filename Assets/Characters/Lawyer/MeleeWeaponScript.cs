using UnityEngine;
using System.Collections.Generic;

public class MeleeWeaponScript : MonoBehaviour
{
    [SerializeField] private LawyerScript script;
    private List<GameObject> damagedEnemies = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (damagedEnemies.Contains(collision.gameObject)) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox"))
        {
            Debug.Log("Melee Weapon hit enemy");

            damagedEnemies.Add(collision.gameObject);

            collision.gameObject.GetComponentInParent<EnemyVariables>().Health -= script.EnemyDamage;
        }
    }
    
    public void ClearDamagedEnemies()
    {
        damagedEnemies.Clear();
    }

}
