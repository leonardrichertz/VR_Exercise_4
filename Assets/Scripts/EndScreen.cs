using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public TMP_Text scoreText;

    void Start()
    {
        scoreText.text = "Dein Score: " + Score.finalScore;
    }
}