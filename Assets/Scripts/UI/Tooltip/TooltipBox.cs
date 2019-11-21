using Kurenaiz.Management.Core;
using TMPro;
using UnityEngine;

public class TooltipBox : MonoBehaviour, IUpdate
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RectTransform box;

    bool on = false;

    private void OnValidate ()
    {
        if (!text)
            text = GetComponentInChildren<TextMeshProUGUI> (true);

        if (!box)
            box = transform.GetChild (0).GetComponent<RectTransform> ();
    }

    private void OnEnable()
    {
        UpdateManager.Subscribe(this);
    }

    private void OnDisable()
    {
        UpdateManager.Unsubscribe(this);
    }

    
    void IUpdate.MUpdate()
    {
        if (on)
            box.transform.position = Input.mousePosition;
    }

    public void ShowTip (string tip, Direction pivot)
    {
        SetDirection (pivot);
        text.text = tip;
        on = true;
        box.gameObject.SetActive (true);
    }

    public void HideTip ()
    {
        on = false;
        box.gameObject.SetActive (false);
    }

    void SetDirection (Direction pivot)
    {
        switch (pivot)
        {
            case Direction.TOP_LEFT:
                box.pivot = new Vector2 (0, 1);
                break;
            case Direction.TOP_RIGHT:
                box.pivot = new Vector2 (1, 1);
                break;
            case Direction.BOTTOM_LEFT:
                box.pivot = new Vector2 (0, 0);
                break;
            case Direction.BOTTOM_RIGHT:
                box.pivot = new Vector2 (1, 0);
                break;
        }
    }
}