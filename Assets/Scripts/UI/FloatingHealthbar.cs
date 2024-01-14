using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthbar : MonoBehaviour
{
    [SerializeField]
    private Slider healthbarSlider;
    

    // Update is called once per frame
    void Update()
    {
    }

    public void SetHealthAmount(int health)
    {
        if (health < 0)
            healthbarSlider.value = 0;
        else if (health > 100)
            healthbarSlider.value = 100;
        else
            healthbarSlider.value = health;
    }

    public void SetMaxHealth(int value)
    {
        Debug.Assert(value > 0);
        healthbarSlider.maxValue = value;
    }
}
