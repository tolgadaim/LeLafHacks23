using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [Serializable]
    public class QuestionSet
    {
        public Transform[] questions;
    }

    public List<QuestionSet> QuestionSets = new List<QuestionSet>();

    public int Score;

    public void EnableQuestionFor(string placeTitle)
    {
        int randomIndex = UnityEngine.Random.Range(0, 3);
        GameObject.FindObjectOfType<TimeManager>().PauseTime();
        switch (placeTitle)
        {
            case "Allentown":
                QuestionSets[0].questions[randomIndex].gameObject.SetActive(true);
                break;
            case "ABE Airport":
                QuestionSets[1].questions[randomIndex].gameObject.SetActive(true);
                break;
            case "Lehigh University":
                QuestionSets[2].questions[randomIndex].gameObject.SetActive(true);
                break;
            case "North Bethlehem":
                QuestionSets[3].questions[randomIndex].gameObject.SetActive(true);
                break;
            case "Steel Stacks":
                QuestionSets[4].questions[randomIndex].gameObject.SetActive(true);
                break;
            case "Lehigh River":
                QuestionSets[5].questions[randomIndex].gameObject.SetActive(true);
                break;
            case "Easton":
                QuestionSets[6].questions[randomIndex].gameObject.SetActive(true);
                break;
            case "Lafayette College":
                QuestionSets[7].questions[randomIndex].gameObject.SetActive(true);
                break;
            default:
                GameObject.FindObjectOfType<TimeManager>().ResumeTime();
                break;
        }
    }

    public void GiveScore()
    {
        Score++;
    }

}
