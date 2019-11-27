using System;
using System.Collections;
using System.Collections.Generic;
using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RightTabController : MonoBehaviour
{
    //TODO: TIGHT SCHEDULE, REFACTOR THIS
    public RectTransform tab;
    public RectTransform sideBar;

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
    bool popup = false;
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

    public void OpenRightBar (bool openHidden)
    {
        open = true;
        rightTab.SetActive (true);

        if (!openHidden)
            StartCoroutine(ExtendTab ());
    }

    public void HideLeftBar() => StartCoroutine(ExtendTab ());

    IEnumerator ExtendTab ()
    {
        yield return new WaitForEndOfFrame();
        if (!popup)
        {
            PopupController.Popup ("'X' fecha a aba, o 'olho' esconde a atmosfera de alguns planetas, nesta aba é mostrado as informações do planeta atualmente orbitado.", 10);
            popup = true;
        }

        EventManager.TriggerEvent ("OnHideRightBar");
        rightTabAnimator.SetTrigger (pressedHash);
    }

    public void UpdateBar (LocalController local)
    {
        if (!open)
            OpenRightBar (false);

        displayPlanet.OnUpdateBar (local.Planet);
        information.OnUpdateBar (local.Data);
        trivia.OnUpdateBar (local.Data);
    }

    public void CloseRightBar ()
    {
        EventManager.TriggerEvent ("OnHideRightBar");
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

    public void ToggleAtmosphere() => displayPlanet.OnToggleAtmosphere();
}