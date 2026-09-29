using System;
using UnityEngine;
using UnityEngine.UI;

public class KeyPickUp : MonoBehaviour
{
    [SerializeField] private Image timeFiller;
    [SerializeField] private int maxTimeFill = 3;

    private float currentTime;

    private bool isStartPicking;

    public static event EventHandler OnKeyPicked;
   
    void Start()
    {
        currentTime = 0f;
        Lander.Instance.KeyPickUp_CanKeyAccess += Lander_CanAccessKey;
    }

    private void Lander_CanAccessKey(object sender, EventArgs e)
    {
        Debug.Log("Lander Can access key.");
        isStartPicking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartPicking)
        {
            currentTime += Time.deltaTime;
            AccessingKey();
        }

    }

    private void AccessingKey()
    {
        Debug.Log("TakenTime " + currentTime);
        if (currentTime < maxTimeFill)
        {
            timeFiller.fillAmount = currentTime / maxTimeFill;
        }
        else
        {
            timeFiller.fillAmount = 1f;
            isStartPicking = false;
            Debug.Log("OnKeyPickedUp Event Calling");
            OnKeyPicked?.Invoke(this, EventArgs.Empty);
            gameObject.SetActive(false);
            timeFiller.fillAmount = 0f;
        }
    }
}
