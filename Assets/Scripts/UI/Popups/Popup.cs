using System.Collections;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    //Animator animator;

    string message;
    float timeToExpire;
    public bool IsPopped { get; private set; }

    private void OnValidate ()
    {
        if (!textMesh)
            textMesh = GetComponentInChildren<TextMeshProUGUI> ();
    }

    public Popup QueueToPop (string message, float timeToExpire)
    {
        IsPopped = true;
        this.message = message;
        this.timeToExpire = timeToExpire;
        return this;
    }

    public void Pop ()
    {
        this.textMesh.text = message;
        gameObject.SetActive (true);
        StartCoroutine (Expire (timeToExpire));
    }

    IEnumerator Expire (float timeToExpire)
    {
        yield return new WaitForSeconds (timeToExpire);
        IsPopped = false;
        gameObject.SetActive (false);
    }

    public void Close ()
    {
        StopAllCoroutines();
        IsPopped = false;
        gameObject.SetActive (false);
    }
}