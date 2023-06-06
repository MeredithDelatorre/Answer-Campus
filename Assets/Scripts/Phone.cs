using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Phone : MonoBehaviour
{
    public TextMeshProUGUI textMessage;
    private string[] messages;

    public GameObject unlockedPhonePanel;
    public GameObject typingPanel;
    

    // Start is called before the first frame update
    void Start()
    {
    }


    public void Notify(string[] msg)
    {
        messages = msg;
        textMessage.text = messages[0];
    }

}
