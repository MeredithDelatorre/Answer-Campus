using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditonalButton : MonoBehaviour
{
    public string PreferenceToCheck;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey(PreferenceToCheck))
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
