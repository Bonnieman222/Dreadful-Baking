using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class StarWarsScrollWithScene : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float scrollSpeed = 30f;
    public float startY = -600f;
    public float endY = 1200f;

    [Header("Scene Settings")]
    public string sceneToLoad;
    public float delayAfterFinish = 5f;

    private RectTransform rectTransform;
    private bool finishedScrolling = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Vector2 pos = rectTransform.anchoredPosition;
        pos.y = startY;
        rectTransform.anchoredPosition = pos;
    }

    void Update()
    {
        if (finishedScrolling) return;

        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (rectTransform.anchoredPosition.y >= endY)
        {
            finishedScrolling = true;
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterFinish);
        SceneManager.LoadScene(sceneToLoad);
    }
}
