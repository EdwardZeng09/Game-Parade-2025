using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePointSystem : MonoBehaviour
{
    public int currentPoints = 0;
    public int maxPoints = 3;

    public TextMeshProUGUI upgraadeText;
    private void Start()
    {
        UpdateUIForUpGrade();
    }

    public void AddPointForUpGrade() 
    {
        if (currentPoints<maxPoints) 
        {
            currentPoints++;
            UpdateUIForUpGrade();
        }
    }

    public void SpendPointForUpGrade() 
    {
        if (currentPoints > 0) 
        {
            currentPoints--;
            UpdateUIForUpGrade(); 
        }
    }
    public void SpendPointForMoudle()
    {
        if (currentPoints > 0)
        {
            currentPoints--;
            UpdateUIForMoudle();
        }
    }
    public void AddPointForMoudle()
    {
        if (currentPoints > 0)
        {
            currentPoints--;
            UpdateUIForMoudle();
        }
    }
    public void UpdateUIForUpGrade()
    {
        upgraadeText.text = $"Available Upgrade Points:  <color=#FF0000>{currentPoints}/{maxPoints}</color>";
    }
    public void UpdateUIForMoudle()
    {
        upgraadeText.text = $"Available Moudle Points: <color=#FF0000>{currentPoints}/{maxPoints}</color>";
    }



}
