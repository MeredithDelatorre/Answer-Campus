/*
 * Letter - Controls behavior of the spawned letters
 */

using UnityEngine;
using VNEngine;

public class Letter : MonoBehaviour {
    private Vector3 targetPosition; // Position where letters will move towards (center)
    private float speed;            // Movement speed of the letter
    private char letterChar;        // The character represented by the letter object
    private LetterSpawner spawner;  // Reference to the LetterSpawner

    // Audio Clips
    [SerializeField] private AudioClip correctClip;
    [SerializeField] private AudioClip incorrectClip;

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

                AudioManager.Instance.PlaySFX(correctClip);

                Destroy(gameObject);
            } else {
                // Incorrect letter
                Debug.Log("Incorrect letter " + letterChar + " reached the center.");

                AudioManager.Instance.PlaySFX(incorrectClip);

                Destroy(gameObject);
            }
        }
    }

    // Detects collisions with the active collider (eraser or tip)
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("EraserCollider")) {

            // Check if the letter is in the word without revealing
            bool letterIsInWord = spawner.IsLetterInWord(letterChar);

            if (letterIsInWord) {
                // If it is in the word, we just erased a correct letter, so play incorrect sound
                AudioManager.Instance.PlaySFX(incorrectClip);
            } else {
                // If it's NOT in the word, erasing is correct
                AudioManager.Instance.PlaySFX(correctClip);
            }

            // Destroy the letter when hitting the eraser in erase mode 
            Destroy(gameObject);

        } else if (other.CompareTag("TipCollider")) {
            // Check if the letter is correct and then destroy it when hitting the pencil tip in writing mode 
            if (spawner.CheckCorrectLetter(letterChar)) {
                AudioManager.Instance.PlaySFX(correctClip);
            } else {
                // In writing mode and letter was incorrect 
                AudioManager.Instance.PlaySFX(incorrectClip);
            }

            Destroy(gameObject);
        }
    }
}