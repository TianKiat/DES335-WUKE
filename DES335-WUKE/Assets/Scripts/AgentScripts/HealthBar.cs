using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;

    private void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    // value = [0,1]
    public void UpdateHealth(float value)
    {
        healthSlider.value = value;
    }
}
