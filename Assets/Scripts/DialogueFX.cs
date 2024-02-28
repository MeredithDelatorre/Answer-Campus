using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFX : MonoBehaviour
{
    public float timeBeforeJiggle;
    private float timer = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= timeBeforeJiggle)
        {
//            GetComponent<Animator>().SetTrigger("jiggle");
            timer = Time.time + timeBeforeJiggle;
        }
        
    }

    public void StartTimer()
    {
        timer = Time.time + timeBeforeJiggle;
    }
    public void StopTimer()
    {
        timer = -1;
    }
}
