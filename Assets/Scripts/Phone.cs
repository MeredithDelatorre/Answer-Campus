using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Phone : MonoBehaviour
{

    private Animator animation;
    private List<TextMessage> messages;
    
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Animator>())
        {
            animation = GetComponent<Animator>();
        }
        else
        {
            Debug.Log("No animator found for phone");
        }

        messages = PlayerPrefsExtra.GetList<TextMessage>("messages", new List<TextMessage>());
        if(HasNewMessages())
        {
            animation.SetTrigger("notification");
        }
    }

    public void ClearNotifications()
    {
        animation.SetTrigger("default");
    }
   

    public bool HasNewMessages()
    {
        return messages.Count > 0;
    }


    public void Notify(string[] msg)
    {

    /*
     * 
     * TODO: Move notifications to a proper PhoneNode behavior
        messages = msg;
        textMessage.text = messages[0];
    */
        }

}
