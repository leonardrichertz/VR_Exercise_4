using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class FinishLineTrigger : MonoBehaviour
{
    public string endSceneName = "EndScene";
    [SerializeField] TMP_Text scoreText;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DisplayScore());
        }
    }

    IEnumerator DisplayScore()
    {
        // show score
        scoreText.text = "Score: " + Score.finalScore;
        Debug.Log(Score.finalScore);
        yield return new WaitForSeconds(3f);
        // hide score
        scoreText.text = "";
        Debug.Log("reload");
        SceneManager.LoadScene(0);
    }
}