using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Phone : MonoBehaviour
{

    public GameObject notificationBadge;
    public TextMeshProUGUI numberOfNewPosts;
    // Start is called before the first frame update
    void Start()
    {
        int totalPosts = PlayerPrefs.GetInt("posts", 0);
        int readPosts = PlayerPrefs.GetInt("posts_read", 0);
        int newPostCount = totalPosts - readPosts;
        numberOfNewPosts.text = newPostCount.ToString();
        if(newPostCount > 0)
        {
            notificationBadge.SetActive(true);
            GetComponent<Animator>().SetTrigger("vibrate");
        }
        else
        {
            notificationBadge.SetActive(false);
        }
    }

    public void CheckedMessages()
    {
        GetComponent<Animator>().SetTrigger("checked");
    }

}
