using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PauseMenuWithIntro : MonoBehaviour
{
    [Header("Pause Menu")]
    public GameObject pauseCanvas;

    [Header("Night Intro")]
    public CanvasGroup introCanvas;
    public float showTime = 5f;

    [Header("Input")]
    public InputActionAsset inputActions;
    public string actionMapName = "Player";
    public string pauseActionName = "Pause";

    [Header("Mobile UI")]
    public GameObject mobilePauseButton; // Assign your mobile pause button here

    [Header("Mobile UI Root")]
    public GameObject mobileUIRoot; // Assign your mobile UI root here

    private InputAction pauseAction;
    public bool isPaused = false;
    private bool allowPause = true;

    private List<GameObject> disabledCanvasObjects = new List<GameObject>();

    // ------------------------------------------------------------
    // MOBILE SETUP
    // ------------------------------------------------------------
    private void Start()
    {
        if (mobilePauseButton != null)
        {
            mobilePauseButton.SetActive(true);
        }
    }

    public void MobilePauseButtonPressed()
    {
        if (!allowPause) return;

        if (isPaused) Resume();
        else Pause();
    }

    // ------------------------------------------------------------
    // INPUT SETUP
    // ------------------------------------------------------------
    private void OnEnable()
    {
        if (inputActions != null)
        {
            var map = inputActions.FindActionMap(actionMapName);
            if (map != null)
            {
                pauseAction = map.FindAction(pauseActionName);
                if (pauseAction != null)
                {
                    pauseAction.performed += OnPausePressed;
                    pauseAction.Enable();
                }
            }
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePressed;
            pauseAction.Disable();
        }
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (!allowPause) return;

        if (isPaused) Resume();
        else Pause();
    }

    // ------------------------------------------------------------
    // PAUSE SYSTEM
    // ------------------------------------------------------------
    public void Pause()
    {
        if (pauseCanvas == null) return;

        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        if (pauseCanvas == null) return;

        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ClosePause()
    {
        if (pauseCanvas != null) pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void DisablePause()
    {
        allowPause = false;
        if (isPaused) ClosePause();
    }

    public void EnablePause()
    {
        allowPause = true;
    }

    // ------------------------------------------------------------
    // NIGHT INTRO SYSTEM (FIXED)
    // ------------------------------------------------------------
    public void PlayIntro()
    {
        StartCoroutine(ShowIntro());
    }

    private IEnumerator ShowIntro()
    {
        DisablePause();
        Time.timeScale = 0f;

        // Disable all other canvas GameObjects
        disabledCanvasObjects.Clear();
        Canvas[] allCanvases = FindObjectsOfType<Canvas>(true);

        foreach (Canvas canvas in allCanvases)
        {
            if (introCanvas != null && canvas.gameObject == introCanvas.gameObject)
                continue;

            if (canvas.gameObject.activeSelf)
            {
                disabledCanvasObjects.Add(canvas.gameObject);
                canvas.gameObject.SetActive(false);
            }
        }

        if (introCanvas != null)
        {
            introCanvas.gameObject.SetActive(true);
            introCanvas.alpha = 1f;
            introCanvas.interactable = true;
            introCanvas.blocksRaycasts = true;
        }

        yield return new WaitForSecondsRealtime(showTime);

        if (introCanvas != null)
        {
            introCanvas.alpha = 0f;
            introCanvas.interactable = false;
            introCanvas.blocksRaycasts = false;
            introCanvas.gameObject.SetActive(false);
        }

        // Restore previously active canvases
        foreach (GameObject obj in disabledCanvasObjects)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // RE-ENABLE mobile UI root
        if (mobileUIRoot != null)
            mobileUIRoot.SetActive(true);

        Time.timeScale = 1f;
        EnablePause();
    }
}
