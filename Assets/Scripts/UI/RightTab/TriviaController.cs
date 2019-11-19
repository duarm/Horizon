using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriviaController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform triviaParent;
    [SerializeField] TextMeshProUGUI[] triviaTexts;

    private void Awake ()
    {
        triviaTexts = triviaParent.GetComponentsInChildren<TextMeshProUGUI> (true);
    }

    public void OnUpdateBar(PlanetData data)
    {
        ShowTrivias(data);
    }

    void ShowTrivias (PlanetData data)
    {
        for (int i = 0; i < triviaTexts.Length; i++)
        {
            if (i >= data.trivias.Count)
            {
                triviaTexts[i].transform.parent.gameObject.SetActive (false);
            }
            else
            {
                triviaTexts[i].text = data.trivias[i];
                triviaTexts[i].transform.parent.gameObject.SetActive (true);
            }
        }
    }

}