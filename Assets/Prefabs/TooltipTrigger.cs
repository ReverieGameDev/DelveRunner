using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    [TextArea] public string body;
    [TextArea] public string secondary;

    public void OnPointerEnter(PointerEventData eventData) => Tooltip.Instance.Show(title, body, secondary);
    public void OnPointerExit(PointerEventData eventData) => Tooltip.Instance.Hide();
}