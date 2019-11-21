using System;
using System.Collections;
using System.Collections.Generic;
using Kurenaiz.Management.Core;
using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;

public class PopupController : MonoBehaviour, IStart
{
    //TODO: TIGHT SCHEDULE, REFACTOR THIS
    private static PopupController Instance;

    [SerializeField] float poppingRate = .7f;
    [SerializeField] AudioClip popupClip;
    [SerializeField] AudioSource source;
    [SerializeField] RectTransform popupsParent;
    [SerializeField] RightTabController rightTab;

    [SerializeField] List<Popup> popups;

    Queue<Popup> popupsQueued = new Queue<Popup> ();
    Coroutine popCoroutine;
    Coroutine repositioningCoroutine;
    WaitForSeconds poppingDelay;

    bool rightTabOpen = false;

    private void OnValidate ()
    {
        if (source)
            source = GetComponent<AudioSource> ();
    }

    private void OnEnable() => UpdateManager.Subscribe(this);

    private void OnDisable() => UpdateManager.Unsubscribe(this);

    private void Awake() => Instance = this;

    void IStart.MStart ()
    {
        poppingDelay = new WaitForSeconds (poppingRate);
        EventManager.StartListening ("OnHideRightBar", UpdatePosition);
    }

    private void UpdatePosition ()
    {
        if (repositioningCoroutine != null)
            StopCoroutine (repositioningCoroutine);

        if (rightTabOpen)
        {
            rightTabOpen = false;
            repositioningCoroutine = StartCoroutine (Reposition (new Vector2 (popupsParent.anchoredPosition.x + (rightTab.tab.rect.width - rightTab.sideBar.rect.width), popupsParent.anchoredPosition.y), .2f));
        }
        else
        {
            rightTabOpen = true;
            repositioningCoroutine = StartCoroutine (Reposition (new Vector2 (popupsParent.anchoredPosition.x - (rightTab.tab.rect.width - rightTab.sideBar.rect.width), popupsParent.anchoredPosition.y), .2f));
        }
    }

    IEnumerator Reposition (Vector2 to, float time)
    {
        var elapsedTime = 0f;
        var startingPos = popupsParent.anchoredPosition;
        while (elapsedTime < time)
        {
            popupsParent.anchoredPosition = Vector2.Lerp (startingPos, to, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public static void Popup(string message, float timeToExpire) => Instance.Pop(message, timeToExpire);

    void Pop (string message, float timeToExpire)
    {
        for (int i = 0; i < popups.Count; i++)
        {
            if (!popups[i].IsPopped)
            {
                popupsQueued.Enqueue (popups[i].QueueToPop (message, timeToExpire));
                if (popCoroutine == null)
                    popCoroutine = StartCoroutine (Popping ());
                break;
            }
        }
    }

    IEnumerator Popping ()
    {
        while (popupsQueued.Count > 0)
        {
            popupsQueued.Dequeue ().Pop ();
            source.PlayOneShot (popupClip);
            yield return poppingDelay;
        }
        popCoroutine = null;
    }
}