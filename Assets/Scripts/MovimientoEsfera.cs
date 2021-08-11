using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoEsfera : MonoBehaviour
{
    public float fuerza = 62;
    public Rigidbody esfera;

    private Vector3 pos_ini_clic, pos_fin_clic;
    private float unidad_pantalla_ajustada;
    private bool ini_clic = false;
    private bool clic_activo = true;

    public ControlNivel controlNivel;

    // Start is called before the first frame update
    void Start()
    {
        unidad_pantalla_ajustada = Screen.height/4;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (clic_activo)
            {
                pos_ini_clic = Input.mousePosition;
                ini_clic = true;
            }        
           
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (clic_activo)
            {
                if (ini_clic)
                {
                    pos_fin_clic = Input.mousePosition;
                    mover();
                    ini_clic = false;
                }
            }
        }
    }

    // Activar por acción dentro del jeugo o por tiempo
    public void activarClic()
    {
        clic_activo = true;
    }

    public void mover()
    {
        clic_activo = false;
        Invoke("activarClic", 1f);

        float a = (pos_fin_clic.y - pos_ini_clic.y);
        float b = (pos_fin_clic.x - pos_ini_clic.x);
        //print("Inicio" + pos_ini_clic);
        //print("Fin" + pos_fin_clic);
        //float pendiente = a / b;
        //float anguloRadianes = Mathf.Atan(pendiente);
        //float anguloGrados = anguloRadianes * Mathf.Rad2Deg;

        Vector2 mouse = new Vector2(pos_ini_clic.x - pos_fin_clic.x, pos_ini_clic.y - pos_fin_clic.y);

        float distanciaclicsy = pos_ini_clic.y - pos_fin_clic.y;

        float unidadesmovidas = distanciaclicsy / unidad_pantalla_ajustada;
        print(unidadesmovidas);
        if (unidadesmovidas > 1.5f)
        {
            unidadesmovidas = 1.5f;
        }


        Vector3 force = new Vector3(mouse.normalized.x, 1, mouse.normalized.y);
        
        esfera.AddForce(force.normalized * fuerza * 100 * unidadesmovidas);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            controlNivel.canvas_victoria.SetActive(true);
            clic_activo = false;
            CancelInvoke("activarClic");
        }
    }


}
