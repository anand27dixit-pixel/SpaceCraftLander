using System.Collections;
using UnityEngine;

public class FuelPickup : MonoBehaviour
{
   [SerializeField] private int fuelMultiplier = 10;

   public int GetFuelMultipler() => fuelMultiplier;

   public void DestroySelf(){
        Destroy(gameObject);
    }
}
