using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenciadorEnemigo : MonoBehaviour
{
    public EnemigoControl personaje;
    private void Start()
    {
        this.GetComponent<Rigidbody>().maxDepenetrationVelocity = 16f;
    }
}
