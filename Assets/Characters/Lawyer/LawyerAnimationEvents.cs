using UnityEngine;

public class LawyerAnimationEvents : MonoBehaviour
{

    [SerializeField] private LawyerScript lawyerScript;
    
    public void SetIsAttacking()
    {
        if (lawyerScript == null)
        {
            return;
        }

        lawyerScript.SetIsAttacking();
    }

    public void SetIsNotAttacking()
    {
        if (lawyerScript == null)
        {
            return;
        }

        lawyerScript.SetIsNotAttacking();
    }
}
