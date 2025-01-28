/*
 * Letter - Controls behavior of the spawned letters
 */

using UnityEngine;

public class Letter : MonoBehaviour {
    private Vector3 targetPosition; // Position where letters will move towards (center)
    private float speed;            // Movement speed of the letter
    private char letterChar;        // The character represented by the letter object
    private LetterSpawner spawner;  // Reference to the LetterSpawner

    public void Initialize(Vector3 targetPosition, float speed, char letterChar, LetterSpawner spawner) {
        this.targetPosition = targetPosition;
        this.speed = speed;
        this.letterChar = letterChar;
        this.spawner = spawner;
    }

    private void Update() {
        // Move letter towards the center
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
            Debug.Log("Reached the center: " + letterChar);

            if (spawner.CheckCorrectLetter(letterChar)) // Check if it is the correct letter
            {
                // Correct letter
                Debug.Log("Correct letter " + letterChar + " reached the center.");
                Destroy(gameObject);
            } else {
                // Incorrect letter
                Debug.Log("Incorrect letter " + letterChar + " reached the center.");
                Destroy(gameObject);
            }
        }
    }

    // Detects collisions with the active collider (eraser or tip)
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("EraserCollider")) {
            // Destroy the letter when hitting the eraser in erase mode 
            Destroy(gameObject);
        } else if (other.CompareTag("TipCollider")) {
            // Check if the letter is correct and then destroy it when hitting the pencil tip in writing mode 
            if (spawner.CheckCorrectLetter(letterChar)) {
                Destroy(gameObject);
            } else {
                // In writing mode and letter was incorrect 
                Destroy(gameObject);
            }
        }
    }
}
