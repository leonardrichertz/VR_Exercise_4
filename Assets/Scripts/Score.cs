using UnityEngine;

public class Score : MonoBehaviour
{
    int score = 0;

    public void RaiseScore()
    {
        score = score + 1;
        Debug.Log(score);
    }
}
