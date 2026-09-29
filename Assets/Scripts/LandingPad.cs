using System.Collections;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
   [SerializeField] private int scoreMultipler=1;

   public int GetScoreMultipler() =>  scoreMultipler; 
}
