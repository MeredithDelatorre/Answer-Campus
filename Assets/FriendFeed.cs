using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendFeed : MonoBehaviour
{
    public Transform content;
    public GameObject postTemplate;
    public Phone phone;
    // Start is called before the first frame update
    void Start()
    {

        int numberOfPosts = PlayerPrefs.GetInt("posts", 0);
        for (int i = 0; i < numberOfPosts; i++)
        {
            Debug.Log("Post #" + i);
            GameObject go = Instantiate(postTemplate, content);
            go.GetComponent<Post>().caption.text = PlayerPrefs.GetString("post_caption_" + i, "N/A");
            go.GetComponent<Post>().poster.text = PlayerPrefs.GetString("post_author_" + i, "N/A");
            go.GetComponent<Post>().profileImage.sprite = Resources.Load<Sprite>("Headshots/" + PlayerPrefs.GetString("post_author_" + i, "Missing"));
            go.GetComponent<Post>().likeCount.text = PlayerPrefs.GetInt("post_" + i + "_likes", 0).ToString("0 likes");

            int charactersInPost = PlayerPrefs.GetInt("post_" + i + "_characters", 0);

            for (int j = 0; j < charactersInPost; j++)
            {
                string characterName = PlayerPrefs.GetString("post_" + i + "_character_" + j);
                GameObject friend = Resources.Load<GameObject>("Friends/Blank");
                friend.GetComponent<Image>().sprite = Resources.Load<Sprite>("Selfies/" + characterName);
                go.GetComponent<Post>().characters.Add(friend);
}
            go.GetComponent<Post>().SetCharacters();


            //Phone Booting (shown for first time), Set Read All Messages

            PlayerPrefs.SetInt("posts_read", numberOfPosts);
            phone.CheckedMessages();

        }
    }

    public void NewPost(GameObject post)
    {
        Instantiate(post, content);
    }

}
