using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace VNEngine
{
    // Not used in real code. Merely a template to copy and paste from when creating new nodes.
    public class NodeCheckpoint : Node
    {
        public int month;
        public int day;
        // Called initially when the node is run, put most of your logic here
        public override void Run_Node()
        {
            StatsManager.Set_Numbered_Stat("month", month);
            StatsManager.Set_Numbered_Stat("day", day);
            CheckpointManager.SaveCheckpoint();
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