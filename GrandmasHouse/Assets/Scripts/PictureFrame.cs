using UnityEngine;
using UnityEngine.EventSystems;

public class PictureFrame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    float targetAngleZ = 10f;
    float resetAngleZ = 0f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.rotation = Quaternion.Euler(0, 0, targetAngleZ);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.rotation = Quaternion.Euler(0, 0, resetAngleZ);
    }
}

