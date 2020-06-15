using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextBoxControl pointsbox;
    public GameObject HighScoreBox;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.points > GameManager.highscore)
        {
            GameManager.highscore = GameManager.points;
            HighScoreBox.SetActive(true);
        }
        else
        {
            HighScoreBox.SetActive(false);
        }

        if (GameManager.questions > GameManager.mostQuestions)
            GameManager.mostQuestions = GameManager.questions;

        int bestStat = int.MinValue;
        double worstStat= int.MaxValue;
        foreach(KeyValuePair<string,int> pair in GameManager.phonemeStats)
        {
            if (bestStat < pair.Value)
            {
                bestStat = pair.Value;
                GameManager.bestPhoneme = pair.Key;
            }
            if (worstStat > pair.Value)
            {
                worstStat = pair.Value;
                GameManager.worstPhoneme = pair.Key;
            }
        }

        GameManager.totalscore += GameManager.points;
        GameManager.timesPlayed++;
            
        pointsbox.SetText("Final score: \n" + GameManager.points.ToString());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
