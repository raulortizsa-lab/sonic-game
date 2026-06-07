using UnityEngine;
using UnityEngine.UI;

public class UI_On : MonoBehaviour
{
    public GameObject imageComponent;
    
    public void ActivateUI()
    { 
        imageComponent.SetActive(true);
    }

  
}
