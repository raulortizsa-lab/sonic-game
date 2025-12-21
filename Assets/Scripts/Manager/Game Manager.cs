using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Singleton

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Si ya existe otro GameManager, destruye este
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public int score; // Variable para los puntos (Numeros enteros).
    public float time; // Variable para el tiempo (Numero decimales).
    public int rings; // Variable para los anillos (Numeros enteros).
    public int lives; // Variable para las vidas (Numeros enteros).

    [Header("Text Ui references")] 
    public TextMeshProUGUI TMP_time;
    public TextMeshProUGUI TMP_score;
    public TextMeshProUGUI TMP_rings;
    public TextMeshProUGUI TMP_lives;

    [Header("Animations references")]
    public Animator AC_rings;

    public UI_Transition_manager TransitionManager;
    
    private void Update()
    {
        timeChronometer();
    }

    public void timeChronometer()
    {
        if (time < 600)
        {
            time = time + Time.deltaTime;

            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);

            TMP_time.text = $"{minutes:0}:{seconds:00}";
        }

        else Debug.Log("TIME OVER");
    }

    public void addScore(int reward)
    {
        score = score + reward;
        TMP_score.text = score.ToString();
    }

    public void addRing()
    {
        rings++;
        TMP_rings.text = rings.ToString();
        AC_rings.SetBool("noRings",false);
    }

    public void cleanRings()
    {
        rings = 0;
        TMP_rings.text = rings.ToString();
        AC_rings.SetBool("noRings",true);
    }

    public void loseLife()
    {
        lives = lives - 1;
        TMP_lives.text = lives.ToString();
        TransitionManager.StartFade(1);

        if (lives == 0)
        {
            TransitionManager.StartFade(1);
            Debug.Log("game over");
        }
    }
}
