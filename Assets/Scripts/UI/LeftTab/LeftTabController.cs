using Kurenaiz.Management.Core;
using Kurenaiz.Management.Events;
using UnityEngine;

public class LeftTabController : MonoBehaviour, IStart
{
    [SerializeField] GameObject leftBar;
    [SerializeField] Animator leftTabAnimator;

    [SerializeField] GameObject helpBar;
    [SerializeField] GameObject optionsBar;
    [SerializeField] GameObject creditsBar;
    [SerializeField] GameObject mainBar;

    [SerializeField] string githubUrl;

    bool popupOpenTab = false;
    bool popupOpenOptions = false;
    bool popupOpenHelp = false;
    bool popupOpenCredits = false;
    readonly int pressedHash = Animator.StringToHash ("Pressed");

    #region LeftTab
    private void OnValidate ()
    {
        if (!leftBar)
            leftBar = transform.GetChild (0).gameObject;
    }

    private void OnEnable ()
    {
        UpdateManager.Subscribe (this);
    }

    private void OnDisable ()
    {
        UpdateManager.Unsubscribe (this);

    }

    void IStart.MStart ()
    {
        EventManager.StartListening ("OnEscape", OpenLeftBar);
    }

    public void OpenLeftBar () => leftBar.SetActive (true);

    public void HideLeftTab ()
    {
        if (!popupOpenTab)
        {
            PopupController.Popup ("No menu da esquerda, pode ser encontrado as opções de Horizon, como a ajuda, opções, créditos e sair.", 10);
            PopupController.Popup ("Abra uma das opções para mais informações.", 11);
            popupOpenTab = true;
        }

        leftTabAnimator.SetTrigger (pressedHash);
    }

    public void OpenOptionsTab ()
    {
        if (!popupOpenOptions)
        {
            PopupController.Popup ("Algumas opções de customização do jogo, passe o mouse sobre a opção para saber mais.", 6);
            popupOpenOptions = true;
        }

        mainBar.SetActive (false);
        helpBar.SetActive (false);
        creditsBar.SetActive (false);
        optionsBar.SetActive (true);
    }

    public void OpenHelpTab ()
    {
        if (!popupOpenHelp)
        {
            PopupController.Popup ("Passe o mouse em cima do interrogação para saber mais sobre a opção", 6);
            popupOpenHelp = true;
        }

        mainBar.SetActive (false);
        optionsBar.SetActive (false);
        creditsBar.SetActive (false);
        helpBar.SetActive (true);
    }

    public void OpenCreditsTab ()
    {
        if (!popupOpenCredits)
        {
            PopupController.Popup ("Os criadores do jogo, e os devidos créditos aos autores de assets gratuitos utilizados no projeto.", 8);
            PopupController.Popup ("Ao clicar no icone do Github, voce sera redirecionado ao repositório do projeto.", 9);
            popupOpenTab = true;
        }

        mainBar.SetActive (false);
        helpBar.SetActive (false);
        optionsBar.SetActive (false);
        creditsBar.SetActive (true);
    }

    public void OpenMainTab ()
    {
        creditsBar.SetActive (false);
        helpBar.SetActive (false);
        optionsBar.SetActive (false);
        mainBar.SetActive (true);
    }

    public void OpenGithub ()
    {
        Application.OpenURL (githubUrl);
    }

    public void Quit ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
    #endregion
}