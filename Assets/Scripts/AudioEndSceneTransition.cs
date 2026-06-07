using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioEndSceneTransition : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Fade")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Escena")]
    [SerializeField] private string sceneToLoad;

    private bool transitionStarted = false;

    private void Start()
    {
        // Aseguramos que la imagen empiece invisible
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    private void Update()
    {
        if (transitionStarted) return;

        // Si el audio existe, ya no está reproduciéndose y sí tiene clip asignado
        if (audioSource != null && audioSource.clip != null && !audioSource.isPlaying)
        {
            transitionStarted = true;
            StartCoroutine(FadeAndChangeScene());
        }
    }

    private IEnumerator FadeAndChangeScene()
    {
        float timer = 0f;

        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        // Aseguramos alpha final en 1
        color.a = 1f;
        fadeImage.color = color;

        SceneManager.LoadScene(sceneToLoad);
    }
}