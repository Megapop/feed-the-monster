using UnityEngine;
using System.Collections;

public class UICircularParticleSystem : MonoBehaviour
{
    public UIParticle particle;


    void FixedUpdate()
    {
        if (Random.value < 0.15f)
            (Instantiate(particle.gameObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject).transform.SetParent(transform);
    }
}
