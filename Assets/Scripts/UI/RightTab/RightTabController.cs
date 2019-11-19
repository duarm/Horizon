using System;
using System.Collections.Generic;
using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RightTabController : MonoBehaviour
{
    [Header ("Configuration")]
    [SerializeField] bool infoIsDefaultTab = true;
    [Header ("References")]
    [SerializeField] GameObject rightTab;
    [SerializeField] Animator rightTabAnimator;
    [SerializeField] InformationController information;
    [SerializeField] TriviaController trivia;
    [SerializeField] DisplayPlanetController displayPlanet;

    [Header ("Tabs")]
    [SerializeField] GameObject infoContent;
    [SerializeField] GameObject triviaContent;
    [SerializeField] GameObject infoSelected;
    [SerializeField] GameObject triviaSelected;

    bool open;
    readonly int pressedHash = Animator.StringToHash ("Pressed");

    private void OnValidate ()
    {
        if (!rightTab)
            rightTab = transform.GetChild (0).gameObject;

        if (!information)
            information = GetComponent<InformationController> ();
        if (!trivia)
            trivia = GetComponent<TriviaController> ();
        if (!displayPlanet)
            displayPlanet = GetComponent<DisplayPlanetController> ();
    }

    private void Start ()
    {
        EventManager.SubscribeToFocus (UpdateBar);

        if (infoIsDefaultTab)
            OpenInfoTab ();
        else
            OpenTriviaTab ();
    }

    public void OpenRightBar ()
    {
        open = true;
        rightTab.SetActive (true);
    }

    public void HideLeftBar ()
    {
        rightTabAnimator.SetTrigger (pressedHash);
    }

    public void UpdateBar (LocalController local)
    {
        if (!open)
            OpenRightBar ();

        displayPlanet.OnUpdateBar (local.Planet);
        information.OnUpdateBar (local.Data);
        trivia.OnUpdateBar (local.Data);
    }

    public void CloseRightBar ()
    {
        rightTab.SetActive (false);
        open = false;
    }

    public void OpenTriviaTab ()
    {
        infoSelected.SetActive (false);
        triviaSelected.SetActive (true);
        infoContent.SetActive (false);
        triviaContent.SetActive (true);
    }

    public void OpenInfoTab ()
    {
        triviaSelected.SetActive (false);
        infoSelected.SetActive (true);
        triviaContent.SetActive (false);
        infoContent.SetActive (true);
    }

    public void ToggleAtmosphere ()
    {
        displayPlanet.OnToggleAtmosphere ();
    }
}