using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    readonly string textFilePath = "Text/";

    readonly string phonemeFileName = "phonemes";
    public List<string> availablePhonemes;
    private List<string> allPhonemes = new List<string>();

    private List<string> allAnswers = new List<string>();

    public Dictionary<string, List<string>> phonemeToAnswer = new Dictionary<string, List<string>>();

    public List<AnswerBoxControl> answerBoxes;
    public TextBoxControl phonemeBox;
    public string currentPhoneme;

    public AnswerBoxControl clickedAnswer;

    public TextBoxControl pointBox;
    //Stuff for stats
    static public int points;
    static public int highscore = 0;
    static public int totalscore = 0;
    static public int timesPlayed = 0;
    static public int questions = 0;
    static public int mostQuestions = 0;
    static public Dictionary<string, int> phonemeStats = new Dictionary<string, int>();
    static public string bestPhoneme = "";
    static public string worstPhoneme = "";

    public int consecutive = 0;

    public TextBoxControl timeBox;
    public int timerstart;
    private int timer;

    private float newGameStartTime;

    private GameObject correctText, wrongText;

    // Start is called before the first frame update
    void Start()
    {
        BuildPhonemesList();
        BuildDictionary();
        PickRandomPhoneme();
        points = 0;
        questions = 0;
        timer = timerstart;
        newGameStartTime = Time.time;
        correctText = GameObject.Find("CorrectBox");
        correctText.SetActive(false);
        wrongText = GameObject.Find("WrongBox");
        wrongText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0) {
            pointBox.SetText(points.ToString());
            timer = timerstart - (int)(Time.time-newGameStartTime);
            if (timer < 0)
                timer = 0;
            timeBox.SetText(timer.ToString());
        }
        else
        {
            //Game is over load new scene
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }

    void BuildPhonemesList()
    {
        //Read in all the phonemes and add to lists
        TextAsset phonemeFile = Resources.Load<TextAsset>(string.Concat(textFilePath,phonemeFileName));
        string[] reader = phonemeFile.text.Split('\n');
        foreach (string line in reader){
            allPhonemes.Add(line);
        }
        availablePhonemes = new List<string>(allPhonemes);
    }

    void BuildDictionary()
    {
        TextAsset file;
        foreach (string phoneme in allPhonemes)
        {
            //If stats aren't being held for this yet add it to the dict
            if (!phonemeStats.ContainsKey(phoneme))
            {
                phonemeStats.Add(phoneme, 0);
            }

            //build the dictionary of phonemes to answers
            phonemeToAnswer.Add(phoneme, new List<string>());
            file = Resources.Load<TextAsset>(string.Concat(textFilePath,phoneme));
            string[] reader = file.text.Split('\n');
            foreach(string line in reader)
            {
                phonemeToAnswer[phoneme].Add(line);
                if (!allAnswers.Contains(line))
                    allAnswers.Add(line);
            }
        }
    }

    public void SetNext()
    {
        //Remove phoneme from available
        availablePhonemes.Remove(currentPhoneme);

        //if all phonemes have been used -> reset list of available
        if (availablePhonemes.Count == 0)
            availablePhonemes = new List<string>(allPhonemes);

        //Increment correct answers for this phoneme
        phonemeStats[currentPhoneme]++;

        //Pick a random phoneme for next set up
        PickRandomPhoneme();

        //Scoring
        points+=100;
        consecutive++;
        if(consecutive >= 3)
        {
            points += 10 * consecutive;
        }

        //Questions
        questions++;

        //Show "Correct!" text
        StartCoroutine("ShowCorrectText");
    }

    public void WrongAnswer()
    {
        //Reset consecutive in gamemanager to be 0
        consecutive = 0;

        //decrement wrong answers for this phoneme
        phonemeStats[currentPhoneme]--;

        //Show "Wrong!" Text
        StartCoroutine("ShowWrongText");
    }

    void PickRandomPhoneme()
    {
        //Pick a random phoneme from the ones that are left
        int index = Random.Range(0, availablePhonemes.Count);
        currentPhoneme = availablePhonemes[index];
        phonemeBox.SetText(currentPhoneme);
        SetAnswers();
    }

    void SetAnswers()
    {
        //Pick a random box for correct answer
        AnswerBoxControl correctBox = answerBoxes[Random.Range(0, answerBoxes.Count)];
        correctBox.correct = true;

        //Pick random correct answer for current phoneme
        List<string> correctAnswers = phonemeToAnswer[currentPhoneme];
        correctBox.SetText(correctAnswers[Random.Range(0, correctAnswers.Count)]);

        //Pick random answers that don't correspond to current phoneme to fill in other boxes
        string otherWrongAnswer = "";
        foreach (AnswerBoxControl wrongBox in answerBoxes)
        {
            if (wrongBox != correctBox)
            {
                wrongBox.correct = false;

                //select random answer, check to see if answer for this current phoneme, if not use it as wrong answer
                string wrongAnswer = allAnswers[Random.Range(0, allAnswers.Count)];
                while (phonemeToAnswer[currentPhoneme].Contains(wrongAnswer)|| wrongAnswer == otherWrongAnswer)
                {
                    wrongAnswer = allAnswers[Random.Range(0, allAnswers.Count)];
                }
                otherWrongAnswer = wrongAnswer;
                wrongBox.SetText(wrongAnswer);
            }
        }
    }

    IEnumerator ShowCorrectText()
    {
        correctText.SetActive(true);
        yield return new WaitForSeconds(3f);
        correctText.SetActive(false);
    }

    IEnumerator ShowWrongText()
    {
        wrongText.SetActive(true);
        yield return new WaitForSeconds(3f);
        wrongText.SetActive(false);
    }
}
