using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct ProfilePicture
{
    public Character character; 
    public Sprite picture;
}

public class TextMessageList : MonoBehaviour
{
    public GameObject listItemTemplate;
    public ProfilePicture[] profiles;    

    // Start is called before the first frame update
    void Start()
    {
        List<TextMessage> messages = PlayerPrefsExtra.GetList<TextMessage>("messages", new List<TextMessage>());
        for(int i = 0; i < messages.Count; i++)
        {
            GameObject go = Instantiate(listItemTemplate, transform);
            go.GetComponent<ViewTextMessage>().message.text = messages[i].message;
            Button button = go.GetComponent<Button>();
            string loc = messages[i].location;
            button.onClick.AddListener(() => GoToLocation(loc));
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

    private void GoToLocation(string location)
    {
        Debug.Log("Going to " + location);
        SceneManager.LoadScene(location);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
