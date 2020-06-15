using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public TextBoxControl HighScore, QuestionsAnswered, AverageScore, BestPhoneme, WorstPhoneme;

    // Start is called before the first frame update
    void Start()
    {
        HighScore.SetText(string.Concat("Highscore: ", GameManager.highscore));
        QuestionsAnswered.SetText(string.Concat("Most Questions Answered: ", GameManager.mostQuestions));
        if (GameManager.timesPlayed != 0)
            AverageScore.SetText(string.Concat("Average Score: ", (GameManager.totalscore / GameManager.timesPlayed)));
        else
            AverageScore.SetText("Average Score: 0");
        if (GameManager.bestPhoneme != "")
            BestPhoneme.SetText(string.Concat("Best Phoneme: ", GameManager.bestPhoneme));
        else
            BestPhoneme.SetText("Best Phoneme: N/A");
        if (GameManager.worstPhoneme != "")
            WorstPhoneme.SetText(string.Concat("Worst Phoneme: ", GameManager.worstPhoneme));
        else
            WorstPhoneme.SetText("Worst Phoneme: N/A");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
