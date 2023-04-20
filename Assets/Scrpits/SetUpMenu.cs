using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetUpMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI MenuText;
    [SerializeField] private TextMeshProUGUI FirstBtnText;
    [SerializeField] private TextMeshProUGUI SecondBtnText;
    [SerializeField] private TextMeshProUGUI ThirdBtnText;
    [SerializeField] private GameObject Info;
    [SerializeField] private TextMeshProUGUI AdditionalText;

    public bool MenuDisplayed = false;

    private void Start()
    {
        //menu.SetActive(false);
    }

    public void SetUpGuidance(string menuText, string firstBtnText, string secondBtnText, string thirdBtnText, string additionalText = null)
    {
        menu.SetActive(true);
        MenuText.text = menuText;
        FirstBtnText.text = firstBtnText;
        SecondBtnText.text = secondBtnText;
        ThirdBtnText.text = thirdBtnText;
        AdditionalText.text = additionalText;
        
        if (additionalText != null)
        {
            Info.SetActive(false);
        }
        MenuDisplayed = true;
    }

    public void CloseGuidance()
    {
        menu.SetActive(false);
        MenuDisplayed = false;
    }
}
