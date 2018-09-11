using UnityEngine;
using System.Collections;

public class ParticlesController : MonoBehaviour
{
    void Update()
    {
        if (GetComponent<ParticleSystem>().isStopped)
            Destroy(gameObject);
    }
}
