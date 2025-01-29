using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Overrides the base LetterSpawner to reveal all occurrences of the guessed letter in the targetWord
/// and introduces directional letter spawning.
/// </summary>
public class RevealAllLetterSpawner : LetterSpawner {
    // Store the unconfirmed letters set here so we can access it in CheckCorrectLetter
    private HashSet<int> unconfirmedLetters;

    protected override IEnumerator SpawnLetters() {
        // Create a HashSet to hold the letters that haven't been confirmed yet
        unconfirmedLetters = new HashSet<int>();
        for (int i = currentIndex; i < targetWord.Length; i++) {
            unconfirmedLetters.Add(i);
        }

        // While the word is not finished, have a 25% chance of spawning a needed letter
        while (unconfirmedLetters.Count > 0) {
            bool spawnNeededLetter = Random.Range(0f, 1f) < 0.25f && unconfirmedLetters.Count > 0;

            char letterToSpawn;
            if (spawnNeededLetter) {
                // Randomly select a needed letter by copying to an array of indices and picking randomly
                int[] indices = new int[unconfirmedLetters.Count];
                unconfirmedLetters.CopyTo(indices);
                int randIndex = indices[Random.Range(0, indices.Length)];
                letterToSpawn = targetWord[randIndex];
            } else {
                letterToSpawn = alphabet[Random.Range(0, alphabet.Length)];
            }

            Vector3 spawnPosition = GetSpawnPosition();
            GameObject letterObj = Instantiate(letterPrefab, spawnPosition, Quaternion.identity);
            letterObj.GetComponentInChildren<TextMeshPro>().text = letterToSpawn.ToString();
            letterObj.GetComponent<Letter>().Initialize(centerCircle.position, letterTravelSpeed, letterToSpawn, this);

            spawnedLetters.Add(letterObj);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public override bool CheckCorrectLetter(char letter) {
        // Get the current underline text as a char array
        char[] underlineChars = targetWordUnderline.text.ToCharArray();
        bool foundAtLeastOneMatch = false;

        // We'll need a list of indices to remove from unconfirmedLetters
        List<int> matchedIndices = new List<int>();

        // Loop through the target word to reveal all matching letters
        for (int i = 0; i < targetWord.Length; i++) {
            if (targetWord[i] == letter && underlineChars[i] == '_') {
                underlineChars[i] = letter;
                foundAtLeastOneMatch = true;
                // Collect this index to remove later
                matchedIndices.Add(i);
            }
        }

        // Remove matched indices from unconfirmedLetters
        foreach (int idx in matchedIndices) {
            unconfirmedLetters.Remove(idx);
        }

        // If we revealed any letters, update the underline UI
        if (foundAtLeastOneMatch) {
            targetWordUnderline.text = new string(underlineChars);

            // If the word is fully revealed (no underscores left)
            if (!targetWordUnderline.text.Contains("_")) {
                // Word is complete, clear letters and start next word after a delay
                ClearAllLetters();
                StartCoroutine(WaitAndStartNextWord());
            }
        }

        return foundAtLeastOneMatch;
    }

    // Directional letter spawner - spawns letters in four directions instead of random angles
    protected override Vector3 GetSpawnPosition() {
        float[] angles = { 0f, 90f, 180f, 270f }; // Fixed angles for directional spawning
        float angle = angles[Random.Range(0, angles.Length)] * Mathf.Deg2Rad;
        float radius = 10f; // Fixed radius for spawn positions
        return new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
    }

    public bool IsLetterInWord(char letter) {
        // Check if the letter is still unrevealed in the word
        for (int i = 0; i < targetWord.Length; i++) {
            // If it's in targetWord and not revealed yet (underscore in the underline)
            if (targetWord[i] == letter && targetWordUnderline.text[i] == '_') {
                return true;
            }
        }
        return false;
    }
}
