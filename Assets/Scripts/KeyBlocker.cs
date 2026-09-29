using System;
using UnityEngine;
using UnityEngine.UI;

public class KeyBlocker : MonoBehaviour
{
    [SerializeField] private Image leftblocker;
    [SerializeField] private Image rightblocker;
    [SerializeField] private int maxTimeFill = 1;

    private float timeSpent = 1f;
    private bool canStart = false;


    // Start is called before the first frame update
    void Start()
    {
        timeSpent = maxTimeFill;
        KeyDrop.OnKeyDropped += KeyPickDrop_KeyDropped;
    }

    private void KeyPickDrop_KeyDropped(object sender, EventArgs e)
    {
        Debug.Log("Key Pick Up Dropped");
        canStart = true;
    }

    void Update()
    {
        if (leftblocker.fillAmount == 0f || rightblocker.fillAmount == 0f)
            return;

        if (timeSpent > 0 && canStart)
        {
            timeSpent -= Time.deltaTime;
            float fillAmount = timeSpent / maxTimeFill;
            leftblocker.fillAmount = fillAmount;
            rightblocker.fillAmount = fillAmount;
        }
    }


}
