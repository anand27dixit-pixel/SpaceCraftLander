using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statUIText;
    [SerializeField] private GameObject speedUpArrow;
    [SerializeField] private GameObject speedDownArrow;
    [SerializeField] private GameObject speedLeftArrow;
    [SerializeField] private GameObject speedRightArrow;

    [SerializeField] private Image fuelBar;

    void Start()
    {
       speedUpArrow?.SetActive(false);
       speedDownArrow?.SetActive(false);
       speedLeftArrow?.SetActive(false);
       speedRightArrow?.SetActive(false);
    }
    // Start is called before the first frame update
    void Update()
    {
        if(statUIText!=null)
        SetStatForGame();
    }

    private void SetStatForGame()
    {

       speedUpArrow.SetActive(Lander.Instance.GetSpeedY()>0);
       speedDownArrow.SetActive(Lander.Instance.GetSpeedY()<=0);
       speedLeftArrow.SetActive(Lander.Instance.GetSpeedX()<=0);
       speedRightArrow.SetActive(Lander.Instance.GetSpeedX()>0);

       fuelBar.fillAmount = Lander.Instance.GetFuelNormalziedAmount();
       
        statUIText.text = "\n" +  GameManager.Instance.GetCurrentLevel() + "\n"
        +  GameManager.Instance.GetScore() + "\n"
        + Mathf.Round(GameManager.Instance.GetTime()) + "\n"
        + Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX() * 10)) + "\n"
        + Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY() * 10)) + "\n";
    }
  
}
