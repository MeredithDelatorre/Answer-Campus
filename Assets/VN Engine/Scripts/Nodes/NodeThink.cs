using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace VNEngine
{
    // Not used in real code. Merely a template to copy and paste from when creating new nodes.
    public class NodeThink : Node
    {
        public GameObject thinkingCanvas;
        public TextMeshProUGUI display;

        private bool thinking = true;

        public string[] thoughts;
        public float timeBetweenThoughts = 5.0f;

        private float nextThoughtTime;
        private int thoughtIndex = 0;
        

        private Vector3[] desiredPosition;

        // Called initially when the node is run, put most of your logic here
        public override void Run_Node()
        {
            thinkingCanvas.SetActive(true);
            StartCoroutine(Thinking_Coroutine());
            
            //            VNSceneManager.scene_manager.Show_UI(false);  // Ensure dialogue panel is visible

            //          running = true;
            //            UIManager.ui_manager.GetComponentInParent<Think>().show();

            // if there's no need to  wait for other operations/coroutines, call finish node at the end of this method
            Finish_Node();
        }


        // What happens when the user clicks on the dialogue text or presses spacebar? Either nothing should happen, or you call Finish_Node to move onto the next node
        public override void Button_Pressed()
        {
            //Finish_Node();
        }


        // Fades the image from opaque to transparent
        IEnumerator Thinking_Coroutine()
        {

            while (thinking)
            {
                Debug.Log("NEXT THOUGHT TIME: " + nextThoughtTime);

                if (nextThoughtTime <= Time.time)
                {
                    Debug.Log("Setting next thought");

                    // Check if there are more thoughts to display
                    if (thoughtIndex < thoughts.Length)
                    {
                        display.text = thoughts[thoughtIndex];
                        thoughtIndex++;
                    }

                    // Set the time for the next thought
                    nextThoughtTime = Time.time + timeBetweenThoughts;
                    Debug.Log("NEXT THOUGHT TIME: " + nextThoughtTime);
                }

                // Check if all thoughts are displayed
                if (thoughtIndex >= thoughts.Length)
                {
                    thinking = false;
                    Debug.Log("All thoughts displayed.");
                }
                if(thinking)
                {
                    // Yield for the next frame
                    yield return null;

                }
                else
                {
                    Finish_Node();
                }
            }

            /*
            Actor[] actors = ActorManager.actors_on_scene.ToArray();
            desiredPosition = new Vector3[actors.Length];

            for (int i = 0; i < actors.Length; i++)
            {
                desiredPosition[i] = actors[i].desired_position;
                if (thinking)
                {
//                    actors[i].Place_At_Position()
                    desiredPosition[i].y = -Screen.height * 2f;

                }
                else
                {
                    desiredPosition[i].y = +Screen.height * 2f;

                }
                actors[i].desired_position += desiredPosition[i];
                Debug.Log("ACTOR POSITION: " + actors[i].desired_position);

            }
            */
        }





        // Do any necessary cleanup here, like stopping coroutines that could still be running and interfere with future nodes
        public override void Finish_Node()
        {
            if(!thinking)
            {
                StopAllCoroutines();
                thinkingCanvas.SetActive(false);

            }

            base.Finish_Node();
        }
    }
}