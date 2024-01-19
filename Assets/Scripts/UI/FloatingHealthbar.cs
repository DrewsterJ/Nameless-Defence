using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public Slider healthBarSlider;

    public void SetHealth(int value)
    {
        healthBarSlider.value = value;
    }

    public void SetMaxHealth(int value)
    {
        Debug.Assert(value >= 0);
        healthBarSlider.maxValue = value;
    }

    public void SetMinHealth(int value)
    {
        Debug.Assert(value >= 0);
        healthBarSlider.minValue = value;
    }
}
