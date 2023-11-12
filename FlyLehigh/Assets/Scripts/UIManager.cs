using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text rightAnswers;
    [SerializeField]
    private TMP_Text time;

    [SerializeField]
    private TMP_Text speed;
    [SerializeField]
    private TMP_Text altitude;

    // Required references
    [SerializeField]
    private AerodynamicController ac;
    [SerializeField]
    private QuestionManager questionManager;
    [SerializeField]
    private TimeManager timeManager;

    private void Start()
    {
        ac = FindObjectOfType<AerodynamicController>();
    }

    private void Update()
    {
        speed.text = string.Format("SPD\n{0:N0}", ac.GetComponent<Rigidbody>().velocity.magnitude * 1.944f); // knots
        altitude.text = string.Format("ALT\n{0:N0}", ac.transform.position.y * 3.281f); // feet

        float seconds = timeManager.TimeSpentFlying;
        time.text = string.Format("Time: {0:D2}:{1:D2}:{2:00.00}", (int)seconds / 3600, (int)((seconds % 3600) / 60), seconds);
        rightAnswers.text = string.Format("Correct Answers: {0:D2}", questionManager.Score);
    }
}
