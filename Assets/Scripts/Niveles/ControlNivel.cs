using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlNivel : MonoBehaviour
{
    [Header("Anexos UI")]
    public GameObject canvas_victoria, canvas_inicio, canvas_principal, canvas_perdida;
    public TextMeshProUGUI txt_resultado;
    public TextMeshProUGUI txt_numero_nivel, txt_modedas, txt_monedas_totales, txt_monedas_nivel, txt_puesto;
    public GameObject meta_obj;
    public Slider sliderProgreso;
    public int avgFrameRate;
    public Text texto_frame;

    [Header("Anexos Funcionales")]
    public GameObject pelota;

    private ControlJuego controljuego;
    private float distancia_nivel;
    [HideInInspector]
    public int monedas_nivel = 0;
    [HideInInspector]
    public bool yacomenzoeljuego = false;

    public bool yaganoelpersonaje = false;



    public void Start()
    {
        controljuego = GameObject.FindGameObjectWithTag("ControlJuego").GetComponent<ControlJuego>();
        txt_numero_nivel.text = (controljuego.nivelActualReal+1).ToString();
        controljuego.pos = 1;
        Invoke("setupPrimario", 0.2f);
        distancia_nivel = controljuego.lista_largo_niveles[controljuego.nivelActual];
    }
    public void setupPrimario()
    {
        txt_monedas_nivel.text = controljuego.monedas_totales.ToString();
    }
    public void colocarMonedas(int numeromonedas)
    {
        monedas_nivel += numeromonedas;
    }
    public void actualizarMonedas()
    {
        txt_modedas.text = monedas_nivel.ToString();
        controljuego.monedas_totales += monedas_nivel;
        txt_monedas_totales.text = controljuego.monedas_totales.ToString();
        txt_monedas_nivel.text = controljuego.monedas_totales.ToString();
        PlayerPrefs.SetInt("monedas", controljuego.monedas_totales);
    }

    // Start is called before the first frame update
    public void reiniciar()
    {
        SceneManager.LoadScene(controljuego.lista_escenas_niveles[controljuego.nivelActual]);
    }
    public void siguienteNivel()
    {
        if (!controljuego.yatermineniveles)
        {
            if (controljuego.nivelActual < 5)
            {
                controljuego.nivelActual++;
                controljuego.nivelActualReal++;
                SceneManager.LoadScene(controljuego.lista_escenas_niveles[controljuego.nivelActual]);
            }
            else
            {
                controljuego.nivelActualReal++;
                controljuego.yatermineniveles = true;
                int ran = Random.Range(0, 4);
                controljuego.nivelActual = ran;
                SceneManager.LoadScene(controljuego.lista_escenas_niveles[controljuego.nivelActual]);
            }
        }       
        else
        {
            controljuego.nivelActualReal++;
            int ran = Random.Range(0,4);
            controljuego.nivelActual = ran;
            SceneManager.LoadScene(controljuego.lista_escenas_niveles[controljuego.nivelActual]);
        }
       
    }
    private void Update()
    {
        sliderProgreso.value = (pelota.transform.position.z * 100) / distancia_nivel;
    }

}
