using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveProgress : MonoBehaviour
{
    [System.Serializable]
    public struct CharacterRelationship
    {
        public string character;
        public Relationship relationship;

    }

    public string nextScene;
    [SerializeField]
    public CharacterRelationship[] relationships;
    
    public void SetNextScene()
    {
        PlayerPrefs.SetString("Next Scene", nextScene);
    }

    public void UpdateFriendship()
    {
        for(int i = 0; i < relationships.Length; i++)
        {
            PlayerPrefs.SetInt(relationships[i].character, (int)relationships[i].relationship);

        }
    }

    public void Save()
    {
        UpdateFriendship();
        SetNextScene();
    }
}
