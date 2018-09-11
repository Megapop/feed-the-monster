using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour
{
    public float After = 2.5f;
    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= After)
            Destroy(gameObject);
    }
}
