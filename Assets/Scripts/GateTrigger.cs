using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    [SerializeField] private Score score;


    void OnTriggerEnter(Collider player)
    {
        if (score != null)
        {
            score.RaiseScore();
        }
    }
}