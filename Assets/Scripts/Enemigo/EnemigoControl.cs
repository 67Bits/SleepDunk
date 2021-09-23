using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoControl : MonoBehaviour
{
    [Header("Ajustes")]
    public float rango_inferior_tiempolanzanmiento, rango_superior_tiempolanzanmiento;
    public float multiplicador_de_fuerzabase_inferior, multiplicador_de_fuerzabase_superior;

    [HideInInspector]
    public List<GameObject> objetospegados;
    [HideInInspector]
    public List<GameObject> piscinasTocadas;
    [HideInInspector]
    public List<GameObject> objetosTocadosRegistro;

    public ControlEsferaIA movimiento_esfera;
    public float fuerzaBase;

    // Start is called before the first frame update
    void Start()
    {
        movimiento_esfera.actualizarFuerzaBase(fuerzaBase, rango_inferior_tiempolanzanmiento,
            rango_superior_tiempolanzanmiento,
            multiplicador_de_fuerzabase_inferior,
            multiplicador_de_fuerzabase_superior);
        objetospegados = new List<GameObject>();

    }
    public void despegarObjetos()
    {
        for (int i = 0; i < objetospegados.Count; i++)
        {
            objetospegados[i].GetComponent<CharacterJoint>().breakForce = 0;
            objetospegados[i].GetComponent<CharacterJoint>().breakTorque = 0;

        }
        objetospegados = new List<GameObject>();
    }

    public bool tengoPiscinaDentro(GameObject piscinap)
    {
        bool respuesta = false;
        for (int i = 0; i < piscinasTocadas.Count; i++)
        {
            if (piscinasTocadas[i] == piscinap)
            {
                respuesta = true;
                return respuesta;
            }
        }
        return respuesta;
    }
    public bool tengoObjetoRegistradoDentro(GameObject objetop)
    {
        bool respuesta = false;
        for (int i = 0; i < objetosTocadosRegistro.Count; i++)
        {
            if (objetosTocadosRegistro[i] == objetop)
            {
                respuesta = true;
                return respuesta;
            }
        }
        return respuesta;
    }
}
