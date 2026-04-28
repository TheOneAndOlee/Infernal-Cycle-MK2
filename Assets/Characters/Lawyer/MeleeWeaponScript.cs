using UnityEngine;
using System.Collections.Generic;

public class MeleeWeaponScript : MonoBehaviour
{
    [SerializeField] private LawyerScript script;
    [SerializeField] private float enemyDamage;
    private List<GameObject> damagedEnemies = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (damagedEnemies.Contains(collision.gameObject)) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox"))
        {
            Debug.Log("Melee Weapon hit enemy");

            damagedEnemies.Add(collision.gameObject);

            var enemy = collision.gameObject.transform.parent.gameObject;

            enemy.GetComponent<EnemyVariables>().Health -= (enemyDamage * PlayerVariables.Instance.DamageMultiplier);

            if (ItemManager.Instance != null)
            {
                ItemManager.Instance.TriggerEnemyDamagedEffects(enemy);
            }
        }
    }
    
    public void ClearDamagedEnemies()
    {
        damagedEnemies.Clear();
    }

}
