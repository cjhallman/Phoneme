using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsecutiveBoxControl : MonoBehaviour
{
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.consecutive >= 3)
        {
            GetComponentInChildren<TextBoxControl>().SetText(string.Concat(gm.consecutive," in a row!"));
        }
        else
        {
            GetComponentInChildren<TextBoxControl>().SetText("");
        }
    }
}
