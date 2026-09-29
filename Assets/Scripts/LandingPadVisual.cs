using UnityEngine;
using TMPro;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreMultiplyerText;
  
   
    // Start is called before the first frame update
    private void Awake()
    {
       scoreMultiplyerText.text = string.Concat("x", GetComponent<LandingPad>().GetScoreMultipler());    
    }
 
}
