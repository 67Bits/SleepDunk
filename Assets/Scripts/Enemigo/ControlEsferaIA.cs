using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEsferaIA : MonoBehaviour
{
    public Rigidbody esfera;

    private bool puedomoverme = true;

    public ControlNivel controlNivel;
    private ControlJuego controljuego;
    public EnemigoControl personaje_control;
    public float angulo_lanzamiento_base;
    private float valor_vector_angulo_lanzamiento_base;

    private float rango_inferior_tiempolanzanmiento, rango_superior_tiempolanzanmiento;
    private float multiplicador_de_fuerzabase_inferior, multiplicador_de_fuerzabase_superior;

    // Control Fuerza
    private float fuerzaActual;
    private float fuerzaBase;
    private float porcentajePiscina = 0.15f;
    private float porcentajeIncremento = 0.05f;
    private float porcentajeReduccion = 0.05f;
    // Control Mancha
    [HideInInspector]
    public int nivelMancha = 0;
    private int limiteAumento = 3;
    // Control Material Personaje
    public SkinnedMeshRenderer personaje_material;
    public Material limpio, nivel1mancha, nivel2mancha, nivel3mancha, nivel4mancha;


    private bool puedotocarrampa = true;
    private bool yapasemeta = false;

    // Start is called before the first frame update
    void Start()
    {
        controljuego = GameObject.FindGameObjectWithTag("ControlJuego").GetComponent<ControlJuego>();
        Invoke("mover", Random.Range(1f, 1.4f));
        valor_vector_angulo_lanzamiento_base = angulo_lanzamiento_base / 45f;
    }

    public void actualizarFuerzaBase(float fuerza, float ran_in_ti, float ran_sup_ti, float ran_in_fu, float ran_su_fu)
    {
        fuerzaBase = fuerza;
        fuerzaActual = fuerza;
        rango_inferior_tiempolanzanmiento = ran_in_ti;
        rango_superior_tiempolanzanmiento = ran_sup_ti;
        multiplicador_de_fuerzabase_inferior = ran_in_fu;
        multiplicador_de_fuerzabase_superior = ran_su_fu;
        personaje_material.material = limpio;
    }
    public void manchar()
    {
        if (nivelMancha == 0)
        {
            personaje_material.material = nivel1mancha;
            // Textura mancha 1
        }
        else if (nivelMancha == 1)
        {
            personaje_material.material = nivel2mancha;
            // Textura mancha 2
        }
        else
        {
            personaje_material.material = nivel3mancha;
            // Textura mancha 3
        }
        if (nivelMancha < 3)
        {
            nivelMancha++;
            limiteAumento++;
            float reduc = porcentajeReduccion * fuerzaActual;
            fuerzaActual -= reduc;
        }
    }

    public void aumentarPorTocarAgua()
    {
        if (nivelMancha == 4)
        {
            personaje_material.material = nivel3mancha;
            // Textura mancha 3
        }
        else if (nivelMancha == 3)
        {
            personaje_material.material = nivel2mancha;
            // Textura mancha 2
        }
        else if (nivelMancha == 2)
        {
            personaje_material.material = nivel1mancha;
            // Textura mancha 1
        }
        else
        {
            personaje_material.material = limpio;
            // Textura sin macha
        }
        if (nivelMancha <= 3 && nivelMancha > 0)
        {
            nivelMancha--;
            float aume = porcentajeIncremento * fuerzaActual;
            fuerzaActual += aume;
        }
    }

    public void aumentarPorObjeto()
    {
        float aume = porcentajeIncremento * fuerzaActual;
        fuerzaActual += aume;
    }
    public void caerEnPiscinaMancha()
    {
        personaje_material.material = nivel3mancha;
        // Textura todo manchado
        nivelMancha = 3;
        float reduc = porcentajePiscina * fuerzaActual;
        fuerzaActual -= reduc;
    }
    public void caerEnPiscinaAgua()
    {
        personaje_material.material = limpio;
        // Textura limpio
        nivelMancha = 0;
        float aumen = porcentajePiscina * fuerzaActual;
        fuerzaActual += aumen;
        personaje_control.despegarObjetos();
    }

    public void activartocarrampa()
    {
        puedotocarrampa = true;
    }

    // Activar por acción dentro del jeugo o por tiempo
    public void mover()
    {
        
        if (controlNivel.yacomenzoeljuego && !yapasemeta)
        {
            if (estacercadelsuelo())
            {
                float factorfuerza = Random.Range(multiplicador_de_fuerzabase_inferior, multiplicador_de_fuerzabase_superior);
                Vector2 factordireccion = new Vector2(Random.Range(-0.8f, 0.8f), Random.Range(0.5f, 1));

                Vector3 force = new Vector3(factordireccion.normalized.x, valor_vector_angulo_lanzamiento_base, factordireccion.normalized.y);

                esfera.AddForce(force.normalized * fuerzaBase * 100 * factorfuerza);
            }
        }

        if (!yapasemeta)
        {
            Invoke("mover", Random.Range(rango_inferior_tiempolanzanmiento, rango_superior_tiempolanzanmiento));
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            if (!controlNivel.yaganoelpersonaje)
            {
                if (!yapasemeta)
                {
                    controljuego.perdi = true;
                    controljuego.pos++;
                }             
            }
            yapasemeta = true;
        }
        else if (other.gameObject.layer == 12)
        {
            lanzarPorRampa();

            personaje_control.despegarObjetos();
        }
        else if (other.gameObject.layer == 20)
        {
            if (nivelMancha > 0)
            {
                if (!personaje_control.tengoObjetoRegistradoDentro(other.gameObject))
                {
                    other.gameObject.GetComponent<ObjetoEscenario>().crearPesaMala();
                    personaje_control.objetosTocadosRegistro.Add(other.gameObject);
                }
            }
            else
            {
                aumentarPorObjeto();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 16)
        {
            if (!personaje_control.tengoPiscinaDentro(collision.gameObject))
            {
                manchar();
                personaje_control.piscinasTocadas.Add(collision.gameObject);
            }
        }
        else if (collision.gameObject.layer == 17)
        {
            personaje_control.despegarObjetos();
            if (!personaje_control.tengoPiscinaDentro(collision.gameObject))
            {
                aumentarPorTocarAgua();
                personaje_control.piscinasTocadas.Add(collision.gameObject);
            }
        }
        else if (collision.gameObject.layer == 19)
        {
            personaje_control.despegarObjetos();
            if (!personaje_control.tengoPiscinaDentro(collision.gameObject))
            {
                caerEnPiscinaAgua();
                personaje_control.piscinasTocadas.Add(collision.gameObject);
            }
        }
        else if (collision.gameObject.layer == 18)
        {
            if (!personaje_control.tengoPiscinaDentro(collision.gameObject))
            {
                caerEnPiscinaMancha();
                personaje_control.piscinasTocadas.Add(collision.gameObject);
            }
        }
    }
    public void lanzarPorRampa()
    {
        if (!yapasemeta)
        {
            Vector3 force = new Vector3(0, 1, 1);
            esfera.AddForce(force.normalized * fuerzaBase * 100 * 1f);
        }
      
    }
    public bool estacercadelsuelo()
    {
        bool respuesta = false;

        int mask = (1 << 13) | (1 << 14) | (1 << 16) | (1 << 17) | (1 << 18) | (1 << 19);
        RaycastHit hit;
        var ray = new Ray(transform.position, Vector3.down); ;
        if (Physics.Raycast(ray, out hit, 0.7f, mask))
        {
            respuesta = true;
        }

        return respuesta;
    }

}
