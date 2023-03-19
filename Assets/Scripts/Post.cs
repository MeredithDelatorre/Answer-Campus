using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Post : MonoBehaviour
{
    public TextMeshProUGUI poster;
    public Image profileImage;
    public Image background;
    public List<GameObject> characters;
    public Transform characterPosition;
    public TextMeshProUGUI caption;
    public TextMeshProUGUI likeCount;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetCharacters()
    {

        for(int i = 0; i < characters.Count; i++)
        {
            Instantiate(characters[0], characterPosition);
        }
    }
}
