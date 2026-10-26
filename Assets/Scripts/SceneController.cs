using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Multiplayer.Center.Common;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private SoundController soundController;
    [SerializeField] private InputActionReference startGameAction;

    [Header("Scripts paused while the overlay is visible")]
    [SerializeField] private Behaviour[] gameplayScripts;

    private bool gameStarted;
    private bool gameOver;
    private bool winner;
    private bool endSequenceStarted;
    public bool GameOver => gameOver;

    private void Awake()
    {
        gameStarted = false;
        gameOver = false;
        winner = false;

        startScreen.SetActive(true);
        victoryScreen.SetActive(false);
        deathScreen.SetActive(false);

        Time.timeScale = 0f;
        AudioListener.pause = true;

        foreach (Behaviour script in gameplayScripts)
        {
            if (script != null)
            {
                script.enabled = false;
            }
        }
    }

    private void OnEnable()
    {
        startGameAction.action.Enable();
    }

    private void Update()
    {
        if (!gameStarted &&
            startGameAction.action.WasPressedThisFrame())
        {
            BeginGame();
        }

        if (gameOver && !endSequenceStarted)
        {
            endSequenceStarted = true;
            StartCoroutine(ShowEndScreenAfterDelay());
        }

        if (gameOver &&
            startGameAction.action.WasPressedThisFrame() &&
            endSequenceStarted)
        {
            //Time.timeScale = 1f;
            //AudioListener.pause = false;
            SceneManager.LoadScene(0);
        }
    }

    private void BeginGame()
    {
        gameStarted = true;

        startScreen.SetActive(false);
        soundController.Play(SoundController.SoundEffect.GameStart);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        foreach (Behaviour script in gameplayScripts)
        {
            if (script != null)
            {
                script.enabled = true;
            }
        }
    }

    public void EndGame(bool playerWin)
    {
        if (gameOver) return;

        gameOver = true;
        winner = playerWin;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private IEnumerator ShowEndScreenAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);

        foreach (Behaviour script in gameplayScripts)
        {
            if (script != null)
            {
                script.enabled = false;
            }
        }

        if (winner)
        {
            soundController.Play(SoundController.SoundEffect.GameVictory);
            victoryScreen.SetActive(true);
        }
        else
        {
            soundController.Play(SoundController.SoundEffect.BossLaugh);
            deathScreen.SetActive(true);
        }

        Time.timeScale = 0f;
        //AudioListener.pause = true;
    }

}