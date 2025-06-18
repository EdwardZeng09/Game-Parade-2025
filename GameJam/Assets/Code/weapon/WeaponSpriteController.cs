using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpriteController : MonoBehaviour
{
    public Sprite idleSprite;    // gun_0
    public Sprite fireSprite;    // gun_1
    public float flashDuration = 0.1f;

    private SpriteRenderer sr;
    private Coroutine flashCoroutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = idleSprite;
    }

    public void FlashOnFire()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        sr.sprite = fireSprite;
        yield return new WaitForSeconds(flashDuration);
        sr.sprite = idleSprite;
    }
}
