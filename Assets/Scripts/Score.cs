using UnityEngine;

public class Score : MonoBehaviour
{
    private int score = 0;
    public static int finalScore = 0;

    public void RaiseScore()
    {
        score++;
        finalScore = score;
        Debug.Log("Score: " + score);
    }
}