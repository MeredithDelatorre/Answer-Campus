using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace VNEngine
{
    public class NodeThink : Node
    {
        public Thought thought;
        public Sprite characterImage;
        public TranslatedText[] thoughts;
        public float timeBetweenThoughts = 5.0f;

        private bool thinking = true;
        private float nextThoughtTime;
        private int thoughtIndex = 0;

        // Called initially when the node is run, put most of your logic here
        public override void Run_Node()
        {
            thought.gameObject.SetActive(true);
            thought.SetCharacter(characterImage);
            thought.SetThought(thoughts[thoughtIndex]);
            thought.ResetColors();
            StartCoroutine(Thinking_Coroutine());
        }

        // Fades the image from opaque to transparent
        IEnumerator Thinking_Coroutine()
        {
            while (thinking)
            {
                if (nextThoughtTime <= Time.time)
                {
                    // Check if there are more thoughts to display
                    if (thoughtIndex < thoughts.Length - 1)
                    {
                        if(thoughtIndex != 0)
                        {
                            Debug.Log("Fading Out Previous Thought");
                            // Fade out previous thought
                            thought.FadeOutText();
                        }
                        yield return new WaitForSeconds(timeBetweenThoughts);
                    }

                    thoughtIndex++;
                    
                    if(thoughtIndex >= thoughts.Length)
                    {
                        thinking = false;
                    }
                    else
                    {
                        thought.SetThought(thoughts[thoughtIndex]);
                        // Set the time for the next thought
                        nextThoughtTime = Time.time + timeBetweenThoughts;
                    }
                }

                yield return null;
            }

            // All thoughts displayed, finish node
            Finish_Node();
        }

        // Do any necessary cleanup here
        public override void Finish_Node()
        {
            thought.gameObject.SetActive(false);
            base.Finish_Node();
        }
    }
}
