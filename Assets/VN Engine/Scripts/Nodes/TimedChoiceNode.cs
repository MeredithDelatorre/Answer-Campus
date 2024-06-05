using System.Collections;
using UnityEngine;

namespace VNEngine
{
    public class TimedChoiceNode : Node
    {
        public float timer = 30;
        public ConversationManager default_choice;
        private ChoiceNode choiceNode;
        private Coroutine timerCoroutine;

        private void Start()
        {
        }


        public override void Run_Node()
        {
            Debug.Log("Timed Choice: " + timer);
            choiceNode = GetComponent<ChoiceNode>();
            if (choiceNode == null)
            {
                Debug.LogError("Timed Choices must include a ChoiceNode Component");
                return;
            }
            timerCoroutine = StartCoroutine(Timer());
        }

        public void StopTimer()
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }
        }

        private IEnumerator Timer()
        {
            Debug.Log("Timer Running...");
            yield return new WaitForSeconds(timer);
            Finish_Node();
//            UIManager.ui_manager.choice_panel.SetActive(false); // Assuming UIManager.ui_manager is a valid reference
//            Debug.Log("Starting Next Conversation");
        }

        public override void Finish_Node()
        {
            Debug.Log("Finishing Timed Choice Node");
            choiceNode.Clear_Choices();        // Hide the UI
            VNSceneManager.current_conversation.Finish_Conversation();
//            default_choice.Start_Conversation();

//            base.Finish_Node();     // Continue conversation
        }

    }
}
