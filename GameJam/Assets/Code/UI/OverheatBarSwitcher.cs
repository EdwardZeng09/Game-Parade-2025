using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverheatBarSwitcher : MonoBehaviour
{
    public Sprite normalSprite;     
    public Sprite overheatSprite;   

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = normalSprite;
    }

    public void SetOverheatState(bool isOverheated)
    {
        image.sprite = isOverheated ? overheatSprite : normalSprite;
    }
}
