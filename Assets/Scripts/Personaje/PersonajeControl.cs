using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeControl : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> objetospegados;
    [HideInInspector]
    public List<GameObject> piscinasTocadas;
    [HideInInspector]
    public List<GameObject> objetosTocadosRegistro;

    public MovimientoEsfera movimiento_esfera;


    public float fuerzaBase;
    public float porcentajeSalto;
    public float porcentajeReduccion;
    public float distacia_del_suelo;

    // Start is called before the first frame update
    void Start()
    {
        movimiento_esfera.actualizarDatos(fuerzaBase, porcentajeSalto, porcentajeReduccion, distacia_del_suelo);
        objetospegados = new List<GameObject>();
    }
    public void despegarObjetos()
    {
        for (int i = 0; i < objetospegados.Count; i ++)
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
