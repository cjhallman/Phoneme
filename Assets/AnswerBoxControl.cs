using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerBoxControl : MonoBehaviour
{
    public bool correct = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string t)
    {
        GetComponentInChildren<UnityEngine.UI.Text>().text = t;
    }

    public string GetText()
    {
        return GetComponentInChildren<UnityEngine.UI.Text>().text;
    }
}
