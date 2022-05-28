using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




[System.Serializable]

public struct PossiblePost
{
    public Sprite backgroundPhoto;
    public string caption;
    public string author;
    public int likeCount;
    public Sprite[] charactersInPhoto;


}

public class FrienderMonitor : MonoBehaviour
{

    public PossiblePost[] possiblePosts;
    public bool RemoveAllPosts = false;

    void Start()
    {
        if(RemoveAllPosts)
        {
            //TODO: Redo this routine so that only the specific player prefs about posts are affected
            PlayerPrefs.DeleteAll();
        }
    }
    public void Post(int postNumber)
    {
        int postIndex = PlayerPrefs.GetInt("posts", 0);

        if (possiblePosts.Length >= postNumber)
        {
            //save post
            PlayerPrefs.SetString("post_background_photo_" + postIndex, possiblePosts[postNumber].backgroundPhoto.name);
            PlayerPrefs.SetString("post_caption_" + postIndex, possiblePosts[postNumber].caption);
            PlayerPrefs.SetString("post_author_" + postIndex, possiblePosts[postNumber].author);
            PlayerPrefs.SetInt("post_" + postIndex + "_characters", possiblePosts[postNumber].charactersInPhoto.Length);
            PlayerPrefs.SetInt("post_" + postIndex + "_likes", possiblePosts[postNumber].likeCount);
            for(int i = 0; i < possiblePosts[postNumber].charactersInPhoto.Length; i++)
            {
                PlayerPrefs.SetString("post_" + postIndex + "_character_" + i, possiblePosts[postNumber].charactersInPhoto[i].name);
            }

        }
        else
        {
            Debug.Log("Chosen post index out of range!");
        }
        Debug.Log("Created Post #" + postIndex);
        postIndex++;
        PlayerPrefs.SetInt("posts", postIndex);

    }
}
