using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneScript : MonoBehaviour
{
    public TextMeshProUGUI display;
    public string[] sentences;
    public float timeBetweenSentences;
    public string sceneToLoad;
    private float nextSentenceTime;
    private int sentenceIndex = 0;
    private int loadSequence = 0;
    // Start is called before the first frame update
    void Start()
    {
        loadSequence = 0;
        if(sentences.Length > 0)
        {
            Debug.Log("Setting first text");
            display.text = sentences[sentenceIndex];
        }

        nextSentenceTime = Time.time + timeBetweenSentences;
        sentenceIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextSentenceTime <= Time.time && loadSequence == 0)
        {
            Debug.Log("Setting next text");

            nextSentenceTime = Time.time + timeBetweenSentences;
            if(sentenceIndex < sentences.Length)
            {
                display.text = sentences[sentenceIndex];
            }

            sentenceIndex++;
        }
        //go over by one to gain another timer on the last item
        if (sentences.Length < sentenceIndex)
        {
            loadSequence = 1;
        }

        if(loadSequence == 1)
        {
            SceneManager.LoadScene(sceneToLoad);
            loadSequence = 2;
                 
        }

    }
}
