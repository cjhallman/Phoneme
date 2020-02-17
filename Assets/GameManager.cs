using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    readonly string textFilePath = "Assets/Text/";
    readonly string audioFilePath = "Assets/Audio/";

    readonly string phonemeFileName = "phonemes.txt";
    public List<string> availablePhonemes;
    private List<string> allPhonemes = new List<string>();

    private List<string> allAnswers = new List<string>();

    public Dictionary<string, List<string>> phonemeToAnswer = new Dictionary<string, List<string>>();

    public List<AnswerBoxControl> answerBoxes;
    public TextBoxControl phonemeBox;
    public string currentPhoneme;

    // Start is called before the first frame update
    void Start()
    {
        BuildPhonemesList();
        BuildDictionary();
        PickRandomPhoneme();
    }

    // Update is called once per frame
    void Update()
    {
        //check to see if mouse has been clicked
        if (Input.GetMouseButtonDown(0))
        {
            //Get mouse position in realtion to game
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            //If the click was on something
            if (hit.collider != null)
            {
                //If the click was on an answer
                if (hit.collider.gameObject.name == "AnswerBox")
                {
                    AnswerBoxControl clickedAnswer = hit.collider.gameObject.GetComponent<AnswerBoxControl>();
                    //If the answer was correct move to next phoneme
                    if (clickedAnswer.correct)
                        SetNext();
                }
            }
        }

        //TODO: add drag and drop functionality for answers
        if (false)
        {
           
            //Play sound
            //AnswerBoxControl clickedAnswer = hit.collider.gameObject.GetComponent<AnswerBoxControl>();
            //string answer = clickedAnswer.GetText();
            //AudioSource audioSource = GetComponent<AudioSource>();
            //audioSource.clip = Resources.Load<AudioClip>(string.Concat(audioFilePath, answer));
            //audioSource.Play();
        }

    }

    void BuildPhonemesList()
    {
        //Read in all the phonemes and add to lists
        StreamReader reader = new StreamReader(string.Concat(textFilePath,phonemeFileName));
        string line = reader.ReadLine();
        do
        {
            allPhonemes.Add(line);
            line = reader.ReadLine();
        }while (line != null);

        availablePhonemes = new List<string>(allPhonemes);
    }

    void BuildDictionary()
    {
        StreamReader reader;
        foreach (string phoneme in allPhonemes)
        {
            phonemeToAnswer.Add(phoneme, new List<string>());
            reader = new StreamReader(string.Concat(textFilePath, phoneme, ".txt"));
            string line = reader.ReadLine();
            do
            {
                phonemeToAnswer[phoneme].Add(line);
                if (!allAnswers.Contains(line))
                    allAnswers.Add(line);
                line = reader.ReadLine();
            } while (line != null);
        }

    }

    void SetNext()
    {
        //Remove phoneme from available
        availablePhonemes.Remove(currentPhoneme);

        //if all phonemes have been used -> win!
        if (availablePhonemes.Count == 0)
            phonemeBox.SetText("You win!");
        else
        {
            PickRandomPhoneme();
        }
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
        foreach(AnswerBoxControl wrongBox in answerBoxes)
        {
            if (wrongBox != correctBox)
            {
                wrongBox.correct = false;

                //select random answer, check to see if answer for this current phoneme, if not use it as wrong answer
                string wrongAnswer = allAnswers[Random.Range(0, allAnswers.Count)];
                while (phonemeToAnswer[currentPhoneme].Contains(wrongAnswer))
                {
                    wrongAnswer = allAnswers[Random.Range(0, allAnswers.Count)];
                }
                wrongBox.SetText(wrongAnswer);
            }
        }
    }
}
