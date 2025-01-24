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

    protected string targetWord = "";
    protected string targetDefinition;
    protected int currentIndex = 0;
    protected readonly string alphabet = "abcdefghijklmnopqrstuvwxyz";
    protected List<GameObject> spawnedLetters = new List<GameObject>();

    protected virtual void Start()
    {
        if (possibleWords != null && possibleWords.Count > 0)
        {
            int randIndex = Random.Range(0, possibleWords.Count);
            WordDefinition chosen = possibleWords[randIndex];
            targetWord = chosen.word;
            targetDefinition = chosen.definition;
        }
        else
        {
            Debug.LogWarning("No WordDefinition entries found. Using fallback values.");
            targetWord = "fallback";
            targetDefinition = "No definition available.";
        }

        if (targetWordUnderline != null)
            UnderlinedUI();

        if (definitionText != null)
            definitionText.text = "Definition: " + targetDefinition;

        StartCoroutine(SpawnLetters());
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
            if (currentIndex >= targetWord.Length)
            {
                ClearAllLetters();
            }
            return true;
        }
        return false;
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
