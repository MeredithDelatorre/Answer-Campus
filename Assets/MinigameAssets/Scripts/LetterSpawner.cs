/*
 * LetterSpawner - Controls spawning letters for the minigame.
 */

using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class LetterSpawner : MonoBehaviour
{
    public GameObject letterPrefab;     // Prefab for the letters
    public Transform centerCircle;      // Center Circle position, where letters will move towards
    public string targetWord = "";      // The word to be formed
    public float spawnInterval = 1.0f;  // Time interval between spawns

    private int currentIndex = 0;       // Index of the next letter to hit
    private readonly string alphabet = "abcdefghijklmnopqrstuvwxyz";  // Pool of random letters
    private List<GameObject> spawnedLetters = new List<GameObject>(); // List to track spawned letters

    private void Start()
    {
        StartCoroutine(SpawnLetters());
    }

    IEnumerator SpawnLetters()
    {
        while (currentIndex < targetWord.Length) // Continue until the letter is formed entirely 
        {
            // Randomly decide to spawn a random letter or needed letter, currently 25% chance, as long as the word isn't complete yet
            bool spawnNeededLetter = Random.Range(0f, 1f) < 0.25f && currentIndex < targetWord.Length;

            char letterToSpawn;

            if (spawnNeededLetter)
            {
                // Spawn the next required letter
                letterToSpawn = targetWord[currentIndex];
            } else {
                // Spawn a random letter from the alphabet
                letterToSpawn = alphabet[Random.Range(0, alphabet.Length)];
            }

            Vector3 spawnPosition = GetRandomOffScreenPosition(); // Determine where the letter should appear
            GameObject letterObj = Instantiate(letterPrefab, spawnPosition, Quaternion.identity);
            letterObj.GetComponentInChildren<TextMeshPro>().text = letterToSpawn.ToString();
            // Move letters towards center circle with random speed (1 to 3 units/sec))
            letterObj.GetComponent<Letter>().Initialize(centerCircle.position, Random.Range(1f, 3f), letterToSpawn, this);

            spawnedLetters.Add(letterObj); //Add spawned letter to list

            // Wait for the spawn interval before spawning the next letter
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Generates a random off screen position for each letter
    Vector3 GetRandomOffScreenPosition()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float radius = 10f;  // Distance from center
        return new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
    }

    // Check if the letter the player hit is the next letter required 
    public bool CheckCorrectLetter(char letter)
    {
        Debug.Log($"Checking letter: {letter}, current target: {targetWord[currentIndex]}, index: {currentIndex}");

        if (currentIndex < targetWord.Length && targetWord[currentIndex] == letter)
        {
            currentIndex++;
            Debug.Log("Correct! Moving to next letter, new index is " + currentIndex);

            // If the word is completed, clear all letters
            if (currentIndex >= targetWord.Length)
            {
                ClearAllLetters();
            }

            return true;
        }
        Debug.Log("Incorrect! Letter did not match.");
        return false;
    }

    // Clear all letters once player completes the word 
    private void ClearAllLetters()
    {
        Debug.Log("Word completed! Clearing all letters.");

        foreach (GameObject letterObj in spawnedLetters)
        {
            Destroy(letterObj); // Destroy each letter object
        }
        spawnedLetters.Clear(); // Clear the list of references
    }

}