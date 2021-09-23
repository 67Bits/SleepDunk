using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicadorProgreso : MonoBehaviour
{
    public GameObject pelota;
    public Slider sliderProgreso;
    private ControlJuego controljuego;
    private float distancia_nivel;
    // Start is called before the first frame update
    void Start()
    {
        controljuego = GameObject.FindGameObjectWithTag("ControlJuego").GetComponent<ControlJuego>();
        distancia_nivel = controljuego.lista_largo_niveles[controljuego.nivelActual];
    }

    private void Update()
    {
        sliderProgreso.value = (pelota.transform.position.z * 100) / distancia_nivel;
    }

}
