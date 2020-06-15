using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerZoneControl : MonoBehaviour
{
    public bool up;
    // Start is called before the first frame update
    void Start()
    {
        up = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "AnswerBox")
            up = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        up = false;
    }
}
