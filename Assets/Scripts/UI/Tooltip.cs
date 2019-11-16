using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string tip;
    [SerializeField] Direction direction;
    TooltipBox tooltipBox;

    private void Awake ()
    {
        tooltipBox = FindObjectOfType<TooltipBox> ();
        Debug.Log(tooltipBox);
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