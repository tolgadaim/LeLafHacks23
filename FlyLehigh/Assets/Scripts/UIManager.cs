using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject endScreen;
    [SerializeField]
    private TMP_Text score;

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
    
    private MoveToOrigin MTO;

    private void Start()
    {
        ac = FindObjectOfType<AerodynamicController>();
        MTO = FindObjectOfType<MoveToOrigin>();
    }

    private void Update()
    {
        speed.text = string.Format("SPD\n{0:N0}", ac.GetComponent<Rigidbody>().velocity.magnitude * 1.944f); // knots
        altitude.text = string.Format("ALT\n{0:N0}", (MTO.HeightDisplacement + ac.transform.position.y) * 3.281f); // feet

        float seconds = timeManager.TimeSpentFlying;
        time.text = string.Format("Time: {0:D2}:{1:D2}:{2:00.00}", (int)seconds / 3600, (int)((seconds % 3600) / 60), seconds%60);
        rightAnswers.text = string.Format("Correct Answers: {0:D2}", questionManager.Score);
    }

    public void ShowEndScreen()
    {
        endScreen.SetActive(true);
        int scoreValue = (int)((questionManager.Score * 1_000_000) / timeManager.TimeSpentFlying);
        score.text = string.Format("Your Score:\n{0:N0}", scoreValue);
        FindObjectOfType<TimeManager>().PauseTime();
    }

    public void RestartGame()
    {
        GameObject.FindObjectOfType<TimeManager>().ResumeTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
