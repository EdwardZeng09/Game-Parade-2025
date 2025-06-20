using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeatBarUI : MonoBehaviour
{
    public Image heatFillImage;
    public Image baseCapacityImage;
    public WeaponController weapon;

    void Update()
    {
        if (weapon == null || heatFillImage == null) return;

        float fill = weapon.currentHeat / weapon.maxHeat;
        heatFillImage.fillAmount = Mathf.Clamp01(fill);
        if (baseCapacityImage != null)
        {
            float baseFill = weapon.baseMaxHeat / weapon.maxHeat;
            baseCapacityImage.fillAmount = Mathf.Clamp01(baseFill);
        }
    }
}
