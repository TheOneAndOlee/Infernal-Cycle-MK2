using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour
{
    [Tooltip("Lifespan of the bullet in seconds")]
    [SerializeField]
    private float _lifespan = 5f;

    [HideInInspector]
    public float EnemyDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Lifespan());
    }

    private IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(_lifespan);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox")) {
            //Debug.Log("Bullet hit enemy");
            var enemy = collision.gameObject.transform.parent.gameObject;

            enemy.GetComponent<EnemyVariables>().Health -= (EnemyDamage);

            if (ItemManager.Instance != null)
            {
                ItemManager.Instance.TriggerEnemyDamagedEffects(enemy);
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            //Debug.Log("Bullet hit ground");
            Destroy(gameObject);
        }
    }
}
