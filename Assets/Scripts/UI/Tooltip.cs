using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    [SerializeField] string tip;
    [SerializeField] Direction direction;
    TooltipBox tooltipBox;

    private void Awake ()
    {
        tooltipBox = FindObjectOfType<TooltipBox> ();
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        tooltipBox.ShowTip (tip, direction);
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        tooltipBox.HideTip ();
    }
}