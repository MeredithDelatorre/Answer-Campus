using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBooleanGoal : MonoBehaviour
{
    public string goalName;
    // Start is called before the first frame update

    void Start()
    {
        PlayerPrefs.SetInt(goalName, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
