using System.Collections;
using UnityEngine;

namespace VNEngine
{
    public class TimedChoiceNode : MonoBehaviour
    {
        // Add any additional properties or methods specific to your custom choice node
        public float timer = 30;
        public ConversationManager default_choice;
        private ChoiceNode choiceNode;
        // Override the Run_Node method to add custom behavior
        private void Start()
        {
            choiceNode = GetComponent<ChoiceNode>();
            if(!choiceNode)
            {
                Debug.LogError("Timed Choices must include a ChoiceNode Component");
            }
            StartCoroutine(Timer());

        }
        public IEnumerator Timer()
        {
            yield return new WaitForSeconds(timer);
            // Hide choice UI
            UIManager.ui_manager.choice_panel.SetActive(false);
            StopAllCoroutines();
            Debug.Log("Starting Next Conversation");
            default_choice.Start_Conversation();
        }

    }
}
