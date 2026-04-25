using UnityEngine;
using UnityEngine.InputSystem;

public class NurseScript : MonoBehaviour
{
    [Tooltip("The gameobject containing the gun")]
    [SerializeField]
    private GameObject _gun;

    public void OnAttack(InputValue value)
    {
        _gun.GetComponent<ShooterScript>().FireBullet();
    }
}
