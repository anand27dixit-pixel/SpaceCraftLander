using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statTextMesh;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI nextButtonTextLabel;

    private Action nextButtonCLickAction;
    // Start is called before the first frame update
    void Start()
    {
        Lander.Instance.OnLanded += OnLandedListener;
        nextButton?.onClick.AddListener(delegate
        {
            nextButtonCLickAction();
        });

        ShowHideUI(false);
    }

    private void OnLandedListener(object sender, Lander.LandedEventArgs e)
    {
        if (e.landingType == Lander.LANDING_TYPE.SUCCESSFUL_LANDING)
        {
            titleTextMesh.text = "<color=green> !!! SUCCESSFUL LANDING !!! </color>";
            nextButtonTextLabel.text = "Continue!";
            nextButtonCLickAction = GameManager.Instance.OnGoToNextLevel;
        }
        else
        {
            titleTextMesh.text = "<color=red> !!!" + e.landingType.ToString() + "!!! </color>";
            nextButtonTextLabel.text = "Retry!";
            nextButtonCLickAction = GameManager.Instance.OnRetryLevel;
        }

        statTextMesh.text = Mathf.Round(e.landingSpeed * 2f) + "\n"
        + Mathf.Round(e.landingAngle * 100) + "\n"
        + "x" + Mathf.Round(e.landingScoreMultipler) + "\n"
        + e.score;

        ShowHideUI(true);

    }

    private void ShowHideUI(bool stat)
    {
        gameObject.SetActive(stat);
        if (stat)
            nextButton.Select();
    }



}
