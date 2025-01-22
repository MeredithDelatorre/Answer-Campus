/*
 * Letter - Controls behavior of the spawned letters
 */

using UnityEngine;
using System.Collections;


public class Letter : MonoBehaviour
{
    private Vector3 targetPosition; // Position where letters will move towards (center)
    private float speed;            // Movement speed of the letter
    private char letterChar;        // The character represented by the letter object
    private LetterSpawner spawner;  // A reference to the LetterSpawner script

    public void Initialize(Vector3 targetPosition, float speed, char letterChar, LetterSpawner spawner)
    {
        this.targetPosition = targetPosition;
        this.speed = speed;
        this.letterChar = letterChar;
        this.spawner = spawner;
    }

    private void Update()
    {
        // Move letter towards the center
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
                    Debug.Log("Reached the center : " + letterChar);  

            if (spawner.CheckCorrectLetter(letterChar)) // Check if it is the correct letter
            {
                // Correct letter 
                Debug.Log("Correct letter " + letterChar + " reached the center.");
                Destroy(gameObject);
            }
            else
            {
                // Incorrect letter 
                Debug.Log("Incorrect letter " + letterChar + " reached the center.");
                Destroy(gameObject); 
            }
        }
    }


    // Detects collisions with the player targeter and processes result for hit 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTargeter")) // Check if the object it collided with is the targeter
        {
            Debug.Log("Destroying letter hit by targeter: " + letterChar);  
            Destroy(gameObject); 
        }
    }
}

