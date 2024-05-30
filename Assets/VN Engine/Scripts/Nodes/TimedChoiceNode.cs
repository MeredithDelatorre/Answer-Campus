using System.Collections;
using UnityEngine;

namespace VNEngine
{
    public class TimedChoiceNode : MonoBehaviour
    {
        public float timer = 30;
        public ConversationManager default_choice;
        private ChoiceNode choiceNode;
        private Coroutine timerCoroutine;

        private void Start()
        {
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
            yield return new WaitForSeconds(timer);
            UIManager.ui_manager.choice_panel.SetActive(false); // Assuming UIManager.ui_manager is a valid reference
            Debug.Log("Starting Next Conversation");
            default_choice.Start_Conversation();
        }
    }
}
