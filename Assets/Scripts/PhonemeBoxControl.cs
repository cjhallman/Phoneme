using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhonemeBoxControl : MonoBehaviour
{
    public GameObject gm;
    readonly string audioFilePath = "Audio/";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Play sound
        AudioSource audioSource = gm.GetComponent<AudioSource>();
        AudioClip audioClip = Resources.Load<AudioClip>(string.Concat(audioFilePath, GetText()));
        audioSource.PlayOneShot(audioClip);
    }

    public string GetText()
    {
        return GetComponentInChildren<UnityEngine.UI.Text>().text;
    }
}
