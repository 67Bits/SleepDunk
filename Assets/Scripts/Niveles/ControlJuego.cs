using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJuego : MonoBehaviour
{

    //Instancia del objeto
    [HideInInspector]
    public static ControlJuego administrador;

    [Header("Listas funcianamiento niveles")]
    public List<string> lista_escenas_niveles;
    public List<float> lista_largo_niveles;


    [Header("Configuraciones")]
    public int nivelActual = 0;
    public int nivelActualReal = 0;

    [HideInInspector]
    public bool yatermineniveles = false;

    [HideInInspector]
    public int monedas_totales;

    [HideInInspector]
    public bool perdi = false;


    public int pos = 1;
    public void Awake()
    {
        if (administrador == null)
        {
            administrador = this;
            DontDestroyOnLoad(administrador);
        }
        else { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.HasKey("monedas"))
        {
            monedas_totales = PlayerPrefs.GetInt("monedas");
        }
        else
        {
            monedas_totales = 0;
        }
    }
}
