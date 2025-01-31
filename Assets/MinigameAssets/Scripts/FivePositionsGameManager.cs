using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FivePositionsGameManager : MonoBehaviour {
    [Header("Word List")]
    public List<WordDefinition> possibleWords;

    [Header("Scene References")]
    public Transform[] spawnPositions = new Transform[5];
    public Transform[] boxPositions = new Transform[5];
    public TextMeshProUGUI[] boxLetterDisplays = new TextMeshProUGUI[5];
    public TextMeshProUGUI targetDefinitionText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI scoreText; // New score UI reference

    [Header("Prefabs/Assets")]
    public GameObject letterPrefab;
    public AudioClip correctClip;
    public AudioClip incorrectClip;

    [Header("Spawn Settings")]
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;
    [Range(0f, 1f)]
    public float chanceOfCorrectLetter = 0.3f;
    public float letterSpeed = 2f;

    private string targetWord = "";
    private char[] targetLetters = new char[5];
    private bool[] boxFilled = new bool[5];
    private string alphabet = "abcdefghijklmnopqrstuvwxyz";

    private int score = 0; // Score counter

    private void Start() {
        UpdateScoreUI();
        StartNewRound();
    }

    private void StartNewRound() {
        WordDefinition chosenDefinition = SelectRandomFiveLetterWord();
        if (chosenDefinition != null) {
            targetWord = chosenDefinition.word;
            if (targetDefinitionText != null) {
                targetDefinitionText.text = "Definition: " + chosenDefinition.definition;
            }
        } else {
            targetWord = "abcde";
            if (targetDefinitionText != null) {
                targetDefinitionText.text = "Definition: [No 5-letter words available!]";
            }
        }

        for (int i = 0; i < 5; i++) {
            targetLetters[i] = targetWord[i];
            boxFilled[i] = false;
            if (boxLetterDisplays[i] != null) {
                boxLetterDisplays[i].text = " ";
            }
        }

        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine() {
        if (countdownText != null) {
            countdownText.gameObject.SetActive(true);

            countdownText.text = "3";
            yield return new WaitForSeconds(1f);

            countdownText.text = "2";
            yield return new WaitForSeconds(1f);

            countdownText.text = "1";
            yield return new WaitForSeconds(1f);

            countdownText.gameObject.SetActive(false);
        }

        StartCoroutine(SpawnLettersRoutine());
    }

    private IEnumerator SpawnLettersRoutine() {
        while (!AllBoxesFilled()) {
            List<int> unfilledIndices = new List<int>();
            for (int i = 0; i < 5; i++) {
                if (!boxFilled[i]) unfilledIndices.Add(i);
            }

            if (unfilledIndices.Count == 0)
                yield break;

            int randomIndex = unfilledIndices[Random.Range(0, unfilledIndices.Count)];
            char letterToSpawn = Random.value < chanceOfCorrectLetter ? targetLetters[randomIndex] : alphabet[Random.Range(0, alphabet.Length)];

            Transform spawnPos = spawnPositions[randomIndex];
            GameObject newLetter = Instantiate(letterPrefab, spawnPos.position, Quaternion.identity);

            TextMeshPro textComp = newLetter.GetComponentInChildren<TextMeshPro>();
            if (textComp != null) {
                textComp.text = letterToSpawn.ToString();
            }

            LetterMovement letterMovement = newLetter.GetComponent<LetterMovement>();
            letterMovement.Initialize(
                this,
                randomIndex,
                letterToSpawn,
                boxPositions[randomIndex].position,
                letterSpeed
            );

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void OnLetterArrived(int boxIndex, char arrivedLetter, GameObject letterObj) {
        if (boxFilled[boxIndex]) {
            Destroy(letterObj);
            return;
        }

        if (arrivedLetter == targetLetters[boxIndex]) {
            boxFilled[boxIndex] = true;
            if (boxLetterDisplays[boxIndex] != null) {
                boxLetterDisplays[boxIndex].text = arrivedLetter.ToString();
            }

            if (AudioManager.Instance != null && correctClip != null) {
                AudioManager.Instance.PlaySFX(correctClip);
            }

            DestroyLettersOnSameX(letterObj.transform.position.x);
        } else {
            if (AudioManager.Instance != null && incorrectClip != null) {
                AudioManager.Instance.PlaySFX(incorrectClip);
            }
        }

        Destroy(letterObj);

        if (AllBoxesFilled()) {
            score++; // Increase score
            UpdateScoreUI(); // Update score UI
            StartCoroutine(RestartGameRoutine());
        }
    }

    private void DestroyLettersOnSameX(float xPosition) {
        GameObject[] allLetters = GameObject.FindGameObjectsWithTag("Letter");

        foreach (GameObject letter in allLetters) {
            if (Mathf.Abs(letter.transform.position.x - xPosition) < 0.1f) {
                Destroy(letter);
            }
        }
    }

    private IEnumerator RestartGameRoutine() {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 5; i++) {
            if (boxLetterDisplays[i] != null) {
                boxLetterDisplays[i].text = " ";
            }
            boxFilled[i] = false;
        }

        StartNewRound();
    }

    private bool AllBoxesFilled() {
        foreach (bool filled in boxFilled) {
            if (!filled) return false;
        }
        return true;
    }

    private WordDefinition SelectRandomFiveLetterWord() {
        List<WordDefinition> validFiveLetterWords = new List<WordDefinition>();
        foreach (WordDefinition wd in possibleWords) {
            if (wd.word.Length == 5) {
                validFiveLetterWords.Add(wd);
            }
        }

        if (validFiveLetterWords.Count > 0) {
            int randIndex = Random.Range(0, validFiveLetterWords.Count);
            return validFiveLetterWords[randIndex];
        }

        return null;
    }

    private void UpdateScoreUI() {
        if (scoreText != null) {
            scoreText.text = "" + score;
        }
    }
}
