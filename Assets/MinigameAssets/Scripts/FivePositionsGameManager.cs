using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FivePositionsGameManager : MonoBehaviour
{
    [Header("Word List")]
    public List<WordDefinition> possibleWords;

    [Header("Scene References")]
    public Transform[] spawnPositions = new Transform[5];
    public Transform[] boxPositions = new Transform[5];
    public TextMeshProUGUI[] boxLetterDisplays = new TextMeshProUGUI[5];
    public TextMeshProUGUI targetDefinitionText;
    public TextMeshProUGUI countdownText; // Existing short countdown before round starts
    public TextMeshProUGUI scoreText;

    [Header("Prefabs/Assets")]
    public GameObject letterPrefab;
    public AudioClip correctClip;
    public AudioClip incorrectClip;

    [Header("Spawn Settings")]
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;
    [Range(0f, 1f)] public float chanceOfCorrectLetter = 0.3f;
    public float letterSpeed = 2f;

    private string targetWord = "";
    private char[] targetLetters = new char[5];
    private bool[] boxFilled = new bool[5];
    private string alphabet = "abcdefghijklmnopqrstuvwxyz";

    private int score = 0;

    [Header("Timer Settings")]
    public float gameDuration = 60f;            // Total game time in seconds
    public TextMeshProUGUI timerText;           // UI to display remaining time
    public TextMeshProUGUI penaltyText;         // UI to briefly show "-0:20" or similar
    public float penaltyTime = 20f;             // How many seconds to remove on incorrect answer
    public GameObject gameOverPanel;            // Panel to show when time runs out
    [SerializeField] private GameObject gameStuff; // Holds gameplay objects/UI that should hide on Game Over
    public TextMeshProUGUI finalScoreText;      // Display final score on game over panel

    private float timeLeft;
    private bool gameIsOver = false;
    private Coroutine spawnRoutine;

    private void Start()
    {
        // Initialize the score UI
        UpdateScoreUI();

        // Hide penalty text and game-over panel at the start
        if (penaltyText != null) penaltyText.gameObject.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameStuff != null) gameStuff.SetActive(true);

        // Set initial time but DO NOT start the GameTimerCoroutine yet
        timeLeft = gameDuration;
        UpdateTimerUI();

        // Start the first round with the short "3-2-1" countdown
        // Only after this countdown will we start the game timer
        StartCoroutine(CountdownCoroutine());
    }

    /// <summary>
    /// The short "3-2-1" countdown before each new round starts, then begins letter spawning.
    /// </summary>
    private IEnumerator CountdownCoroutine()
    {
        // Set up a new round
        StartNewRound();

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);

            countdownText.text = "3";
            yield return new WaitForSeconds(1f);

            countdownText.text = "2";
            yield return new WaitForSeconds(1f);

            countdownText.text = "1";
            yield return new WaitForSeconds(1f);

            countdownText.gameObject.SetActive(false);
        }

        // Now that the countdown is over, start the timer and spawn letters (only if the game isn't over)
        if (!gameIsOver)
        {
            // Start the timer here instead of in Start()
            StartCoroutine(GameTimerCoroutine());

            // Keep a reference to stop it if game ends
            spawnRoutine = StartCoroutine(SpawnLettersRoutine());
        }
    }

    /// <summary>
    /// Main game timer that counts down from gameDuration to 0.
    /// </summary>
    private IEnumerator GameTimerCoroutine()
    {
        while (timeLeft > 0 && !gameIsOver)
        {
            yield return null;                  // Wait one frame
            timeLeft -= Time.deltaTime;         // Decrement time
            UpdateTimerUI();

            // If time runs out, end the game
            if (timeLeft <= 0 && !gameIsOver)
            {
                timeLeft = 0;  // Clamp at 0
                UpdateTimerUI();
                EndGame();
            }
        }
    }

    /// <summary>
    /// Updates the timerText UI to show minutes:seconds.
    /// </summary>
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeLeft / 60f);
            int seconds = Mathf.FloorToInt(timeLeft % 60f);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

    /// <summary>
    /// Spawns letters at random intervals. Continues until all boxes are filled or the game ends.
    /// </summary>
    private IEnumerator SpawnLettersRoutine()
    {
        while (!AllBoxesFilled() && !gameIsOver)
        {
            // Which boxes are still unfilled?
            List<int> unfilledIndices = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                if (!boxFilled[i]) unfilledIndices.Add(i);
            }

            if (unfilledIndices.Count == 0)
                yield break;

            int randomIndex = unfilledIndices[Random.Range(0, unfilledIndices.Count)];

            // Decide if we spawn a correct letter or random letter
            char letterToSpawn = Random.value < chanceOfCorrectLetter
                ? targetLetters[randomIndex]
                : alphabet[Random.Range(0, alphabet.Length)];

            Transform spawnPos = spawnPositions[randomIndex];
            GameObject newLetter = Instantiate(letterPrefab, spawnPos.position, Quaternion.identity);

            // Set letter text
            TextMeshPro textComp = newLetter.GetComponentInChildren<TextMeshPro>();
            if (textComp != null)
            {
                textComp.text = letterToSpawn.ToString();
            }

            // Initialize movement
            LetterMovement letterMovement = newLetter.GetComponent<LetterMovement>();
            letterMovement.Initialize(
                this,
                randomIndex,
                letterToSpawn,
                boxPositions[randomIndex].position,
                letterSpeed
            );

            // Wait before next spawn
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// Selects a random word from the list, displays its definition, and resets boxes.
    /// </summary>
    private void StartNewRound()
    {
        // Pick a random 5-letter word
        WordDefinition chosenDefinition = SelectRandomFiveLetterWord();
        if (chosenDefinition != null)
        {
            targetWord = chosenDefinition.word;
            if (targetDefinitionText != null)
            {
                targetDefinitionText.text = "Definition: " + chosenDefinition.definition;
            }
        }
        else
        {
            targetWord = "abcde";
            if (targetDefinitionText != null)
            {
                targetDefinitionText.text = "Definition: [No 5-letter words available!]";
            }
        }

        // Reset box state
        for (int i = 0; i < 5; i++)
        {
            targetLetters[i] = targetWord[i];
            boxFilled[i] = false;
            if (boxLetterDisplays[i] != null)
            {
                boxLetterDisplays[i].text = " ";
            }
        }
    }

    /// <summary>
    /// Returns true if all 5 boxes are filled, else false.
    /// </summary>
    private bool AllBoxesFilled()
    {
        foreach (bool filled in boxFilled)
        {
            if (!filled) return false;
        }
        return true;
    }

    /// <summary>
    /// Called by LetterMovement when a letter arrives at a box.
    /// </summary>
    public void OnLetterArrived(int boxIndex, char arrivedLetter, GameObject letterObj)
    {
        if (gameIsOver)  // If the game ended, just destroy letter and stop
        {
            Destroy(letterObj);
            return;
        }

        if (boxFilled[boxIndex])
        {
            // If it's already filled, just destroy the letter (no effect)
            Destroy(letterObj);
            return;
        }

        // Check correctness
        if (arrivedLetter == targetLetters[boxIndex])
        {
            // Correct letter
            boxFilled[boxIndex] = true;
            if (boxLetterDisplays[boxIndex] != null)
            {
                boxLetterDisplays[boxIndex].text = arrivedLetter.ToString();
            }

            // Play correct SFX
            if (AudioManager.Instance != null && correctClip != null)
            {
                AudioManager.Instance.PlaySFX(correctClip);
            }

            // Destroy any letters at the same x position
            DestroyLettersOnSameX(letterObj.transform.position.x);
        }
        else
        {
            // Incorrect letter
            if (AudioManager.Instance != null && incorrectClip != null)
            {
                AudioManager.Instance.PlaySFX(incorrectClip);
            }

            // Apply penalty to the timer
            timeLeft -= penaltyTime;
            if (timeLeft < 0) timeLeft = 0; // clamp
            UpdateTimerUI();

            // Show penalty text briefly
            if (penaltyText != null)
            {
                penaltyText.gameObject.SetActive(true);
                penaltyText.text = string.Format("-0:{0:00}", (int)penaltyTime);
                StartCoroutine(HidePenaltyText());
            }
        }

        // Clean up letter object
        Destroy(letterObj);

        // If all boxes filled, increment score and start a new round
        if (AllBoxesFilled())
        {
            score++;
            UpdateScoreUI();
            StartCoroutine(RestartGameRoutine());
        }

        // If time has dropped to 0, end game
        if (timeLeft <= 0 && !gameIsOver)
        {
            EndGame();
        }
    }

    /// <summary>
    /// Hides the penalty text after a short delay.
    /// </summary>
    private IEnumerator HidePenaltyText()
    {
        yield return new WaitForSeconds(1f);
        if (penaltyText != null)
        {
            penaltyText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Destroy all letters whose x-position is very close to a given value.
    /// </summary>
    private void DestroyLettersOnSameX(float xPosition)
    {
        GameObject[] allLetters = GameObject.FindGameObjectsWithTag("Letter");
        foreach (GameObject letter in allLetters)
        {
            if (Mathf.Abs(letter.transform.position.x - xPosition) < 0.1f)
            {
                Destroy(letter);
            }
        }
    }

    /// <summary>
    /// After a short delay, clear boxes and start a new word round (assuming time remains).
    /// </summary>
    private IEnumerator RestartGameRoutine()
    {
        yield return new WaitForSeconds(2f);

        if (!gameIsOver)
        {
            // Clear box displays
            for (int i = 0; i < 5; i++)
            {
                if (boxLetterDisplays[i] != null)
                {
                    boxLetterDisplays[i].text = " ";
                }
                boxFilled[i] = false;
            }

            // Restart the short countdown for the next round
            StartCoroutine(CountdownCoroutine());
        }
    }

    /// <summary>
    /// Picks a random 5-letter word from the 'possibleWords' list.
    /// </summary>
    private WordDefinition SelectRandomFiveLetterWord()
    {
        List<WordDefinition> validFiveLetterWords = new List<WordDefinition>();
        foreach (WordDefinition wd in possibleWords)
        {
            if (wd.word.Length == 5)
            {
                validFiveLetterWords.Add(wd);
            }
        }

        if (validFiveLetterWords.Count > 0)
        {
            int randIndex = Random.Range(0, validFiveLetterWords.Count);
            return validFiveLetterWords[randIndex];
        }
        return null;
    }

    /// <summary>
    /// Updates the UI for the score counter.
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    /// <summary>
    /// Stops the game, stops all coroutines, and shows the Game Over panel.
    /// </summary>
    private void EndGame()
    {
        gameIsOver = true;
        // Stop spawning letters and clear leftover letters
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        clearLeftoverLetters();

        // Show final score & game over panel
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameStuff != null) gameStuff.SetActive(false);
        if (finalScoreText != null) finalScoreText.text = score + " Words Studied";
    }

    // This can be called by a UI "Retry" button to reset the game
    public void RetryGame()
    {
        // Reset relevant variables
        gameIsOver = false;
        score = 0;
        UpdateScoreUI();
        timeLeft = gameDuration;
        UpdateTimerUI();

        // Hide game over panel
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameStuff != null) gameStuff.SetActive(true);

        // Clear leftover letters
        clearLeftoverLetters();

        // Instead of immediately starting the timer, 
        // we once again call the countdown, which will 
        // start the timer and letter spawning at the right time.
        StartCoroutine(CountdownCoroutine());
    }

    private void clearLeftoverLetters()
    {
        // Clear any leftover letters
        var leftoverLetters = GameObject.FindGameObjectsWithTag("Letter");
        foreach (var letter in leftoverLetters)
        {
            Destroy(letter);
        }
    }
}
