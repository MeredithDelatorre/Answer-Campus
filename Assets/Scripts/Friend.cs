using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Relationship { NONE, NEGATIVE, POSITIVE };
public class Friend : MonoBehaviour
{

    public Relationship relationship;
    public string characterName;
    public string scene;
    public Image status;
    public Sprite[] statusTypes;
    public bool randomize = true;


    // Start is called before the first frame update
    void Start()
    {
        int current_status = PlayerPrefs.GetInt(characterName, 0);
        if(randomize)
        {
            current_status = Random.Range(0, statusTypes.Length);
        }
        status.sprite = statusTypes[current_status];        
    }

}
