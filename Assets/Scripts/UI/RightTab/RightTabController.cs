using System;
using System.Collections.Generic;
using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RightTabController : MonoBehaviour
{
    [SerializeField] bool infoIsDefaultTab = true;
    [SerializeField] GameObject rightTab;
    [SerializeField] Animator rightTabAnimator;
    [SerializeField] InformationController information;
    [SerializeField] TriviaController trivia;

    [Header("Tabs")]
    [SerializeField] GameObject infoContent;
    [SerializeField] GameObject triviaContent;
    [SerializeField] GameObject infoSelected;
    [SerializeField] GameObject triviaSelected;


    private void OnValidate ()
    {
        if (!rightTab)
            rightTab = transform.GetChild (0).gameObject;
    }

    private void Start ()
    {
        EventManager.SubscribeToFocus (OpenRightBar);

        if(infoIsDefaultTab)
            OpenInfoTab();
        else
            OpenTriviaTab();
    }

    public void OpenRightBar (PlanetData data)
    {
        if(data == null)
            return;

        rightTab.SetActive (true);
        information.SetValues (data);
        trivia.ShowTrivias(data);
    }

    public void OpenTriviaTab ()
    {
        infoSelected.SetActive(false);
        triviaSelected.SetActive(true);
        infoContent.SetActive(false);
        triviaContent.SetActive(true);
    }

    public void OpenInfoTab ()
    {
        triviaSelected.SetActive(false);
        infoSelected.SetActive(true);
        triviaContent.SetActive(false);
        infoContent.SetActive(true);
    }
}