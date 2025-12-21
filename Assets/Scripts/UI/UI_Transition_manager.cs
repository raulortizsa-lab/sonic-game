using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Transition_manager : MonoBehaviour
{
    [Header("Panel Reference")]
    [SerializeField] private Image panelImage;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;

    private Coroutine currentFade;

    private void Awake()
    {
        // Seguridad por si se te olvida asignarlo
        if (panelImage == null)
            panelImage = GetComponent<Image>();
    }

    #region Public API

    public void FadeIn()
    {
        StartFade(1f); // Alpha 0 → 1
    }

    public void FadeOut()
    {
        StartFade(0f); // Alpha 1 → 0
    }

    #endregion

    #region Core Logic
    public void StartFade(float targetAlpha)
    {
        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeRoutine(targetAlpha));
    }
    private IEnumerator FadeRoutine(float targetAlpha)
    {
        Color color = panelImage.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            panelImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        panelImage.color = new Color(color.r, color.g, color.b, targetAlpha);
        currentFade = null;
    }
    
    #endregion
}