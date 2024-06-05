using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace VNEngine
{
    // Not used in real code. Merely a template to copy and paste from when creating new nodes.
    public class NodeOutcome : Node
    {
        public string title;
        public string description;
        public float GPA = 4.0f; //not in use
        public TextMessage textMessage;
        public FriendRelationship[] friendRelationships;
        public int minutesElapsed;

        // Called initially when the node is run, put most of your logic here
        public override void Run_Node()
        {
            if(StatsManager.Numbered_Stat_Exists("Minutes Passed"))
            {
                StatsManager.Add_To_Numbered_Stat("Minutes Passed", minutesElapsed);
            }
            else
            {
                StatsManager.Set_Numbered_Stat("Minutes Passed", minutesElapsed);
            }
            for (int i = 0; i < friendRelationships.Length; i++)
            {
                StatsManager.Set_Numbered_Stat(friendRelationships[i].friend, (int)friendRelationships[i].relationship);
            }

            List<TextMessage> messages = PlayerPrefsExtra.GetList<TextMessage>("messages", new List<TextMessage>());
            messages.Add(textMessage);
            PlayerPrefsExtra.SetList("messages", messages);
            Finish_Node();
        }


        // What happens when the user clicks on the dialogue text or presses spacebar? Either nothing should happen, or you call Finish_Node to move onto the next node
        public override void Button_Pressed()
        {
            //Finish_Node();
        }


        // Do any necessary cleanup here, like stopping coroutines that could still be running and interfere with future nodes
        public override void Finish_Node()
        {
            StopAllCoroutines();

            base.Finish_Node();
        }
    }
}