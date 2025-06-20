using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthFillImage;
    public PlayerCharacter player;

    void Update()
    {
        if (player == null || healthFillImage == null) return;

        float fill = player.GetCurrentHealth() / player.maxHealth;
        healthFillImage.fillAmount = Mathf.Clamp01(fill);
    }
}