using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] private int coinMultiplierScore = 100;

   public int GetCoinMultipler() => coinMultiplierScore;

   public void DestroySelf(){
        Destroy(gameObject);
    }
}
