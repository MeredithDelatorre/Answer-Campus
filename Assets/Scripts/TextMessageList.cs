using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;

[System.Serializable]
public struct ProfilePicture
{
    public Character character; 
    public Sprite picture;
}

public class TextMessageList : MonoBehaviour
{
    public GameObject listItemTemplate;
    public GameObject messageTemplate;
    public GameObject inbox;
    public TextMeshProUGUI inboxHeader;
    public Image inboxProfile;
    public Button likeMessageButton;
    
    public ProfilePicture[] profiles;
    private List<TextMessage> messages;
    // Start is called before the first frame update
    void Start()
    {
        messages = PlayerPrefsExtra.GetList<TextMessage>("messages", new List<TextMessage>());
        for(int i = 0; i < messages.Count; i++)
        {
            GameObject go = Instantiate(listItemTemplate, transform);
//            go.GetComponent<ViewTextMessage>().message.text = messages[i].message;
            Button button = go.GetComponent<Button>();
//            string loc = messages[i].location;
            Character from = messages[i].from;
            //change to view message
            //            button.onClick.AddListener(() => GoToLocation(loc));
            button.onClick.AddListener(() => LoadThread(from, messages));
            go.GetComponent<ViewTextMessage>().from.text = messages[i].from.ToString();
            for(int j = 0; j < profiles.Length; j++)
            {
                if(profiles[j].character == messages[i].from)
                {
                    go.GetComponent<ViewTextMessage>().profile.sprite = profiles[j].picture;
                }
            }
        }
    }

    private void ClearMessages()
    {
        ViewTextMessage[] list = inbox.GetComponentsInChildren<ViewTextMessage>();
        Debug.Log("FOUND " + list.Length + "messages");
        for (int i = 0; i < list.Length; i++)
        {
            Destroy(list[i].gameObject);
        }
    }
    private void LoadThread(Character from, List<TextMessage> messages)
    {
        Debug.Log("LOADING THREAD FOR " + from.ToString());
        ClearMessages();
        inboxHeader.text = from.ToString();
        inbox.transform.parent.parent.parent.gameObject.SetActive(true);
        for (int i = 0; i < messages.Count; i++)
        {
            if(messages[i].from == from)
            {
                for (int j = 0; j < profiles.Length; j++)
                {
                    if (profiles[j].character == messages[i].from)
                    {
                        inboxProfile.sprite = profiles[j].picture;
                    }
                }

                //CREATE (MULTIPLE) MESSAGE
                string[] sentences = Regex.Split(messages[i].message, @"(?<=[\.!\?])\s+");
                Debug.Log("FOUND " + sentences.Length + " SENTENCES.");
                for(int j = 0; j < sentences.Length; j++)
                {
                    GameObject go = Instantiate(messageTemplate, inbox.transform);
                    go.GetComponent<ViewTextMessage>().message.text = sentences[j];
                }
                //change to view message
                string loc = messages[i].location;
                likeMessageButton.onClick.AddListener(() => GoToLocation(loc, i));
                likeMessageButton.onClick.AddListener(() => GoToLocation(loc, i));

                //                go.GetComponent<ViewTextMessage>().from.text = messages[i].from.ToString();
                /*
                                for (int j = 0; j < profiles.Length; j++)
                                {
                                    if (profiles[j].character == messages[i].from)
                                    {
                                        go.GetComponent<ViewTextMessage>().profile.sprite = profiles[j].picture;
                                    }
                                }
                */
            }
        }
        transform.parent.parent.parent.gameObject.SetActive(false);
    }

    private void GoToLocation(string location, int index)
    {
        Debug.Log("Going to " + location);
        SceneManager.LoadScene(location);
    }



}
