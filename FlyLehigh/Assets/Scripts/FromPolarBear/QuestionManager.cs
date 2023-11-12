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
            case "Steel Stacks":
                QuestionSets[3].questions[randomIndex].gameObject.SetActive(true);
                break;
            case "Lehigh River":
                QuestionSets[4].questions[randomIndex].gameObject.SetActive(true);
                break;
            case "Easton":
                QuestionSets[5].questions[randomIndex].gameObject.SetActive(true);
                break;
            case "Lafayette College":
                QuestionSets[6].questions[randomIndex].gameObject.SetActive(true);
                break;
        }
    }

    public void GiveScore()
    {
        Score++;
    }

}
