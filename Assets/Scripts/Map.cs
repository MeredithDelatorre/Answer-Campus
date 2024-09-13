using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    public Location[] locations;
    public GameObject locationButtons;
    private List<CharacterLocation> characterLocations;
    public Characters characters;
    // Start is called before the first frame update
    void Start()
        
    {
        //        System.DateTime.Now.ToShortTimeString()
        //        timestamp = Time.time;
        locations = GetComponentsInChildren<Location>();
        characterLocations = PlayerPrefsExtra.GetList<CharacterLocation>("characterLocations", new List<CharacterLocation>());
        for (int i = 0; i < locations.Length; i++)
        {
            if(SceneManager.GetActiveScene().name == locations[i].scene)
            {
                locations[i].GetComponent<Button>().enabled = false;
            }

            foreach (var characterLocation in characterLocations)
            {
                // Check if the character's location matches the active scene
                if (characterLocation.location == locations[i].name)
                {

                    for (int j = 0; j < characters.profiles.Length; j++)
                    {
                        if (characters.profiles[j].character == characterLocation.character)
                        {
                            // Assign the profile picture to the location's UI
                            locations[i].characterWaiting.sprite = characters.profiles[j].picture;
                            locations[i].characterWaiting.gameObject.SetActive(true); // Show the profile picture
                            break; // Exit the loop once the correct profile is found
                        }
                    }

                }
                else
                {
                    locations[i].characterWaiting.gameObject.SetActive(false);

                }
            }


        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
