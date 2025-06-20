using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float fadeSpeed=1f;
    public float lifetime = 1f;

    private TextMeshProUGUI text;
    private Color originalColor;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalColor = text.color;
    }

    public void SetText(string value) 
    {
    text.text = value;
    text.color = originalColor;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
        Color c=text.color;
        c.a-=fadeSpeed* Time.deltaTime;
        text.color = c;


        lifetime-= Time.deltaTime;
        if (lifetime <= 0f) 
        {
        Destroy(gameObject);
        }
    }

}
