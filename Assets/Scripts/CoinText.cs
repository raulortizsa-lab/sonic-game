using UnityEngine;
using TMPro;

public class CoinText : MonoBehaviour
{
    public TextMeshProUGUI text_ref;
    public int currentCoin = 0;
    public AudioSource audioref;
    
    public void sumarContador()
    {
        currentCoin++;
        audioref.Play();
        text_ref.text = "Monedas: " + currentCoin.ToString("00");
    }
}
