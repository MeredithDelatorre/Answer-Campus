using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LetterSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject letterPrefab; // Prefab for the letters
    [SerializeField] protected Transform centerCircle; // Center Circle position
    [SerializeField] protected List<WordDefinition> possibleWords; // List of possible word-definition pairs

    [SerializeField] protected TextMeshProUGUI targetWordText;
    [SerializeField] protected TextMeshProUGUI targetWordUnderline;
    [SerializeField] protected TextMeshProUGUI definitionText;

    [SerializeField] protected float spawnInterval; // Time interval between spawns
    [SerializeField] protected float timeBeforeNextWord = 2f; // Delay before next word

    protected string targetWord = "";
    protected string targetDefinition;
    protected int currentIndex = 0;
    protected readonly string alphabet = "abcdefghijklmnopqrstuvwxyz";
    protected List<GameObject> spawnedLetters = new List<GameObject>();

    protected virtual void Start()
    {
        InitializeGame();
    }

    // Selects a new word, sets up UI, and starts spawning letters
    protected virtual void InitializeGame() {

        PickRandomWord();
        UpdateUI();

        // Reset index for the new word
        currentIndex = 0;

        // Clear any lingering letters 
        ClearAllLetters();

        // Start spawning letters for the new word
        StartCoroutine(SpawnLetters());
    }

    // Picks a random word-definition pair from the list
    protected void PickRandomWord() {

        if (possibleWords != null && possibleWords.Count > 0) {
            int randIndex = Random.Range(0, possibleWords.Count);
            WordDefinition chosen = possibleWords[randIndex];
            targetWord = chosen.word;
            targetDefinition = chosen.definition;
        } else {
            Debug.LogWarning("No WordDefinition entries found. Using fallback word.");
            targetWord = "fallback";
            targetDefinition = "No definition available.";
        }
    }

    // Updates the UI texts for underline and definition
    protected void UpdateUI() {
        // Clear the underline text first
        if (targetWordUnderline != null) {
            targetWordUnderline.text = "";
            UnderlinedUI(); // Build underscore placeholders for each letter.
        }

        // Update definition text
        if (definitionText != null) {
            definitionText.text = "Definition: " + targetDefinition;
        }
    }

    protected virtual IEnumerator SpawnLetters()
    {
        while (currentIndex < targetWord.Length)
        {
            bool spawnNeededLetter = Random.Range(0f, 1f) < 0.25f && currentIndex < targetWord.Length;
            char letterToSpawn = spawnNeededLetter ? targetWord[currentIndex] : alphabet[Random.Range(0, alphabet.Length)];

            Vector3 spawnPosition = GetSpawnPosition();
            GameObject letterObj = Instantiate(letterPrefab, spawnPosition, Quaternion.identity);
            letterObj.GetComponentInChildren<TextMeshPro>().text = letterToSpawn.ToString();
            letterObj.GetComponent<Letter>().Initialize(centerCircle.position, Random.Range(1f, 3f), letterToSpawn, this);

            spawnedLetters.Add(letterObj);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Allow subclasses to define their own spawn positions
    protected virtual Vector3 GetSpawnPosition()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float radius = 10f;
        return new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
    }

    public virtual bool CheckCorrectLetter(char letter)
    {
        if (currentIndex < targetWord.Length && targetWord[currentIndex] == letter)
        {
            if (targetWordUnderline != null)
            {
                char[] underlineChars = targetWordUnderline.text.ToCharArray();
                underlineChars[currentIndex] = letter;
                targetWordUnderline.text = new string(underlineChars);
            }

            currentIndex++;

            // if word is complete, clear all letters and prepare for new word 
            if (currentIndex >= targetWord.Length)
            {
                ClearAllLetters();
                StartCoroutine(WaitAndStartNextWord());
            }
            return true;
        }
        return false;
    }

    // Wait a few seconds, then start the game again with a new word
    protected IEnumerator WaitAndStartNextWord() {
        // Wait for timeBeforeNextWord seconds
        yield return new WaitForSeconds(timeBeforeNextWord);

        // Now initialize a fresh word
        InitializeGame();
    }

    protected virtual void ClearAllLetters()
    {
        foreach (GameObject letterObj in spawnedLetters)
        {
            Destroy(letterObj);
        }
        spawnedLetters.Clear();
    }

    protected virtual void UnderlinedUI()
    {
        for (int i = 0; i < targetWord.Length; i++)
        {
            targetWordUnderline.text += "_";
        }
    }
}
