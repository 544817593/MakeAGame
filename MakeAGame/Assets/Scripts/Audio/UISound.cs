using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QFramework;


public class UISound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
   
    public AudioClip hoverSound;
    public AudioClip clickSound;
   

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
            AudioKit.PlaySound(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null)
            AudioKit.PlaySound(clickSound);
    }

    
}
