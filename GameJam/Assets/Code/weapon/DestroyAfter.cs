using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float delay = 0.3f;
    void Start() => Destroy(gameObject, delay);
}
