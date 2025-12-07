using UnityEngine;
using TMPro;

public class CoinText : MonoBehaviour
{
    public TextMeshProUGUI text_ref;
    public int currentCoin = 0;
    public AudioSource audioref;
    public Animator coinText;
    
    public void sumarContador()
    {
        currentCoin++;
        audioref.Play();
        text_ref.text = currentCoin.ToString("0");
        coinText.SetBool("noRings", false);
    }

    [ContextMenu("damage")]
    public void damage()
    {
        currentCoin = 0;
        text_ref.text = currentCoin.ToString("0");
        coinText.SetBool("noRings", true);
    }
}
