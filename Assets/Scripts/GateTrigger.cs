using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider player)
    {
        Score score = player.GetComponent<Score>();
        score.RaiseScore();
    }
}
