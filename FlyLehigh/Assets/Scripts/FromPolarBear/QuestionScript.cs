using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScript : MonoBehaviour
{
    [SerializeField]
    private QuestionManager qManager;
    [SerializeField]
    private char correctAnswer = 'A';

    public bool CheckIfCorrectAnswer(char answer)
    {
        bool correct = (answer == correctAnswer);
        if (correct)
        {
            qManager.GiveScore();
        }
        StartCoroutine(InvokeCoroutine(ResetQuestion, 1f));
        return correct;
    }

    public void ResetQuestion()
    {
        foreach (Image childImage in GetComponentsInChildren<Image>())
        {   
            childImage.GetComponent<Button>().enabled = true;
            childImage.color = new Color(0.737f, 0.737f, 0.737f);
        }
        GameObject.FindObjectOfType<TimeManager>().ResumeTime();
        transform.gameObject.SetActive(false);
    }

    private IEnumerator InvokeCoroutine(System.Action action, float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        if (action != null)
            action();
    }
}
