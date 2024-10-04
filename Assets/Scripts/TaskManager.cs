using UnityEngine;
using System.Collections.Generic;
using VNEngine;

public class TaskManager : MonoBehaviour
{
    // Singleton instance
    public static TaskManager Instance { get; private set; }

    // Awake method to implement singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist between scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy any extra instances
        }
    }

    // Assign a new task during a conversation
    public void AssignTask(string taskID)
    {
        string taskKey = "Task_" + taskID;
        StatsManager.Set_Boolean_Stat(taskKey + "_Assigned", true);
        Debug.Log("Task assigned: " + taskID);
    }

    // Mark a task as completed
    public void CompleteTask(string taskID)
    {
        string taskKey = "Task_" + taskID;
        if (StatsManager.Get_Boolean_Stat(taskKey + "_Assigned"))
        {
            StatsManager.Set_Boolean_Stat(taskKey + "_Completed", true);
            Debug.Log("Task completed: " + taskID);
        }
        else
        {
            Debug.LogWarning("Trying to complete a task that hasn't been assigned: " + taskID);
        }
    }

    // Retrieve all assigned but incomplete tasks
    public List<string> GetAssignedTasks()
    {
        List<string> activeTasks = new List<string>();

        foreach (KeyValuePair<string, bool> stat in StatsManager.boolean_stats)
        {
            if (stat.Key.EndsWith("_Assigned") && stat.Value)
            {
                string taskID = stat.Key.Replace("_Assigned", "");

                // Check if the task is not completed
                if (!StatsManager.Get_Boolean_Stat(taskID + "_Completed"))
                {
                    activeTasks.Add(taskID);
                }
            }
        }
        return activeTasks;
    }

    // Retrieve all completed tasks
    public List<string> GetCompletedTasks()
    {
        List<string> completedTasks = new List<string>();

        foreach (KeyValuePair<string, bool> stat in StatsManager.boolean_stats)
        {
            if (stat.Key.EndsWith("_Completed") && stat.Value)
            {
                string taskID = stat.Key.Replace("_Completed", "");
                completedTasks.Add(taskID);
            }
        }
        return completedTasks;
    }
}
