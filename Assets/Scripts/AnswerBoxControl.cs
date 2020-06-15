using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerBoxControl : MonoBehaviour
{
    public bool correct = false;
    public GameObject gm;
    public AnswerZoneControl az;
    readonly string audioFilePath = "Audio/";
    Vector3 originallocation;
    //The plane the object is currently being dragged on
    private Plane dragPlane;
    //The difference between where the mouse is on the drag plane and where the origin of the object is on the drag plane
    private Vector3 offset;
    private Camera myMainCamera;
    private GameManager gmCont;
    private float clickstart;
    private string text;

    // Start is called before the first frame update
    void Start()
    {
        myMainCamera = Camera.main; // Camera.main is expensive ; cache it here
        originallocation = transform.localPosition;
        gmCont = gm.GetComponent<GameManager>();
        clickstart = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Turn off bird on other apple
        if(gmCont.clickedAnswer != null)
            gmCont.clickedAnswer.transform.Find("Bird").gameObject.SetActive(false);

        //Turn on bird on this apple
        transform.Find("Bird").gameObject.SetActive(true);

        //Set this as the new clicked answer in the gamemanager
        gmCont.clickedAnswer = this;

        //get stuff for draging
        dragPlane = new Plane(myMainCamera.transform.forward, transform.position);
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);
        dragPlane.Raycast(camRay, out float planeDist);
        offset = transform.position - camRay.GetPoint(planeDist);

        //Set this as start time of click to track how long the click is
        clickstart = Time.time;
    }

    private void OnMouseDrag()
    {
        //Have apple follow draging
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);

        dragPlane.Raycast(camRay, out float planeDist);
        transform.position = camRay.GetPoint(planeDist) + offset;
    }

    private void OnMouseUp()
    {
        //If the user selected this as their answer
        if (az.up)
        {
            //If the answer is correct
            if (correct)
            {
                //Turn off bird
                transform.Find("Bird").gameObject.SetActive(false);

                //Call function in gamemanager to set next phoneme up & do other correct answer functionality
                gmCont.SetNext();
            }
            else
            {
                //Call function in gamemanager to do wrong answer functionality
                gmCont.WrongAnswer();
            }
        }

        //Set az.up back to false to indicate it was recognized as an answer
        az.up = false;

        //Set apple back to original location
        transform.position = originallocation;

        //If it was a short click
        if((Time.time - clickstart) < .2f)
        {
            //Play sound
            AudioSource audioSource = gm.GetComponent<AudioSource>();
            AudioClip audioClip = Resources.Load<AudioClip>(string.Concat(audioFilePath, GetText()));
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void SetText(string t)
    {
        text = t;
    }

    public string GetText()
    {
        return text;
    }

}
