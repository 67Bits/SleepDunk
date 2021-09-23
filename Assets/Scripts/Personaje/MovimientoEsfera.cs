using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MovimientoEsfera : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody esfera;

    // Movimiento 
    private Vector3 pos_ini_clic, pos_fin_clic;
    private float unidad_pantalla_ajustada;
    private bool ini_clic = false;
    public float angulo_lanzamiento_base;
    private float valor_vector_angulo_lanzamiento_base;

    private bool activador_movimiento = false;
    private bool mepuedomover = true;
    private bool mostrarflecha = false;


    // Control
    public ControlNivel controlNivel;
    public ControlJuego controljuego;
    public PersonajeControl personaje_control;

    // Control Fuerza
    private float fuerzaActual;
    private float fuerzaBase;
    private float porcentajeSalto;
    private float porcentajeReduccion;
    private float distacia_del_suelo;

    // Control Mancha
    [HideInInspector]
    public int nivelMancha = 0;
    private int limiteAumento = 3;

    // Control Material Personaje
    public SkinnedMeshRenderer personaje_material;
    public Material limpio, nivel1mancha, nivel2mancha, nivel3mancha, nivel4mancha;


    // Evento de llamado feel halar
    private bool yallamefxhalar = false;
    public EventosFeel eventosfeel;

    // Para cambiar canvas la primera vez que se toca 
    private bool miprimeravez = true;

    // Control de joystick
    public GameObject joystick;
    public GameObject flecha;
    public bool activarflecha;

    // Multiplicador 
    private bool yachoquemultiplicador = false;

    //Camara
    public Animator camarografo;
    public CinemachineImpulseSource vibration;
    public float valor_vibracion_salto, valor_vibracion_piso;
    public bool vibrar_colision = false;
    public Animator splashcamera;

    public bool puedotocarrampa = true;

    public GAManager gamanager;


    /////////////////////////////////// Colisión /////////////////////////////////////////////////////////////
    public void OnTriggerEnter(Collider other)
    {
        // Final 2
        if (other.gameObject.layer == 24)
        {
            StopAllCoroutines();
            mepuedomover = false;
            if (!controljuego.perdi)
            {
                controlNivel.canvas_victoria.SetActive(true);
                controlNivel.canvas_principal.SetActive(false);
                eventosfeel.dormirallelgaralameta();
            }
            else
            {
                controlNivel.canvas_perdida.SetActive(true);
            }
            controlNivel.actualizarMonedas();
            gamanager.OnLevelComplete(controljuego.nivelActual + 1);
        }
        // Final 1
        if (other.gameObject.layer == 11)
        {
            if (!controljuego.perdi)
            {
                controlNivel.yaganoelpersonaje = true;
                controlNivel.colocarMonedas(100);
                controlNivel.txt_monedas_nivel.text = (controljuego.monedas_totales + 100).ToString();
                eventosfeel.generarMonedas();
            }
            else
            {
                controlNivel.colocarMonedas(20);
                controlNivel.actualizarMonedas();
                controlNivel.canvas_perdida.SetActive(true);
                StopAllCoroutines();
                mepuedomover = false;
            }
            if (controljuego.pos == 2)
            {
                controlNivel.txt_puesto.text = controljuego.pos.ToString() + "nd Place";
            }
            else if (controljuego.pos == 3)
            {
                controlNivel.txt_puesto.text = controljuego.pos.ToString() + "rd Place";
            }
            else if (controljuego.pos >= 4)
            {
                controlNivel.txt_puesto.text = controljuego.pos.ToString() + "th Place";
            }
        }
        // Pesas Flotantes
        else if (other.gameObject.layer == 20)
        {
            //if (nivelMancha > 0)
            //{
            //    if (!personaje_control.tengoObjetoRegistradoDentro(other.gameObject))
            //    {
            //        other.gameObject.GetComponent<ObjetoEscenario>().crearPesaMala();
            //        personaje_control.objetosTocadosRegistro.Add(other.gameObject);
            //    }
            //}
            //else
            //{
            //    aumentarPorObjeto();
            //}
        }
        // Rampa
        else if (other.gameObject.layer == 12)
        {
            if (puedotocarrampa)
            {
                esfera.velocity = Vector3.zero;
                puedotocarrampa = false;
                personaje_control.despegarObjetos();
                lanzarPorRampa();
                Invoke("activartocarrampa", 1f);
                GameObject instanciado = Instantiate(eventosfeel.par_rampa, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z + 10), Quaternion.identity) as GameObject;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Charco malo
        if (collision.gameObject.layer == 16)
        {
            if (!personaje_control.tengoPiscinaDentro(collision.gameObject))
            {
                manchar();
                personaje_control.piscinasTocadas.Add(collision.gameObject);
            }
        }
        // Charco Agua
        else if (collision.gameObject.layer == 17)
        {
            personaje_control.despegarObjetos();
            if (!personaje_control.tengoPiscinaDentro(collision.gameObject))
            {
                aumentarPorTocarAgua();
                personaje_control.piscinasTocadas.Add(collision.gameObject);
            }
        }
        // Piscina Buena
        else if (collision.gameObject.layer == 19)
        {
            personaje_control.despegarObjetos();
            if (!personaje_control.tengoPiscinaDentro(collision.gameObject))
            {
                caerEnPiscinaAgua();
                personaje_control.piscinasTocadas.Add(collision.gameObject);
            }
        }
        // Piscina mala
        else if (collision.gameObject.layer == 18)
        {
            if (!personaje_control.tengoPiscinaDentro(collision.gameObject))
            {
                caerEnPiscinaMancha();
                personaje_control.piscinasTocadas.Add(collision.gameObject);
            }
        }
        else // Piso
        if (collision.gameObject.layer == 13)
        {
            metodoColisionconpiso(null);

        }

        // Multiplicador 
        else if (collision.gameObject.layer == 23)
        {
            chocarcollisonador(collision);
        }

    }
    /////////////////////////////////// Fin Colisión /////////////////////////////////////////////////////////////
    void Start()
    {
        controljuego = GameObject.FindGameObjectWithTag("ControlJuego").GetComponent<ControlJuego>();
        unidad_pantalla_ajustada = Screen.height / 8;
        joystick.SetActive(true);

        controljuego.perdi = false;

        valor_vector_angulo_lanzamiento_base = angulo_lanzamiento_base / 45f;

        this.GetComponent<Rigidbody>().maxDepenetrationVelocity = 0.01f;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mepuedomover)
            {
                if (estacercadelsuelo() || activador_movimiento)
                {
                    mostrarflecha = true;
                    activador_movimiento = false;
                    StopAllCoroutines();
                    StartCoroutine(activarMovimiento(true, 4));

                    joystick.transform.localScale = Vector2.one;
                    pos_ini_clic = Input.mousePosition;
                    ini_clic = true;
                    if (!yallamefxhalar)
                    {
                        yallamefxhalar = true;
                        eventosfeel.halar();
                        camarografo.SetTrigger("zoom");

                        if (miprimeravez)
                        {
                            controlNivel.canvas_inicio.SetActive(false);
                            controlNivel.yacomenzoeljuego = true;
                            controlNivel.canvas_principal.SetActive(true);
                            miprimeravez = false;
                        }
                    }
                }
            }
          
        }

        else if (Input.GetMouseButtonUp(0))
        {
            yallamefxhalar = false;
            if (ini_clic)
            {
                mostrarflecha = false;
                joystick.transform.localScale = Vector2.zero;
                pos_fin_clic = Input.mousePosition;
                mover();
                ini_clic = false;
                activarflecha = false;
                Invoke("activarFlecha", 0.4f);
                eventosfeel.salto();
                camarografo.SetTrigger("zoom out");
                Invoke("activarVibracion", 0.6f);
                vibration.m_ImpulseDefinition.m_AmplitudeGain = valor_vibracion_salto;
                vibration.GenerateImpulse();
            }
        }

        if (ini_clic)
        {
            float a = (Input.mousePosition.y - pos_ini_clic.y);
            float b = (Input.mousePosition.x - pos_ini_clic.x);

            if (a < 0)
            {
                float pendiente = a / b;
                float anguloRadianes = Mathf.Atan(pendiente);
                float anguloGrados = anguloRadianes * Mathf.Rad2Deg;


                var angles = transform.rotation.eulerAngles;
                angles.x = -angulo_lanzamiento_base + 90;
                angles.y = (Mathf.Abs(anguloGrados)) - 90;
                if (anguloGrados < 0)
                {
                    angles.y = anguloGrados + 90;
                }
                else
                {
                    angles.y = anguloGrados - 90;
                }
                angles.z = 0;
                angles.y = -angles.y;
                flecha.transform.rotation = Quaternion.Euler(angles);
            }
        }
        else
        {
            if (activarflecha)
            {
                var angles = transform.rotation.eulerAngles;
                angles.x = -angulo_lanzamiento_base + 90;
                angles.z = 0;
                angles.y = 0;
                flecha.transform.rotation = Quaternion.Euler(angles);
            }
        }

        if (estacercadelsuelo() || activador_movimiento || mostrarflecha)
        {
            if (mepuedomover)
            {
                if (activarflecha)
                {
                    if (!flecha.activeSelf)
                    {
                        flecha.SetActive(true);
                    }
                }
            }        
        }
        else
        {
            if (flecha.activeSelf)
            {
                flecha.SetActive(false);
            }
        }
    }
    public void mover()
    {
        float a = (pos_fin_clic.y - pos_ini_clic.y);
        float b = (pos_fin_clic.x - pos_ini_clic.x);

        if (a < 0)
        {
            Vector2 mouse = new Vector2(pos_ini_clic.x - pos_fin_clic.x, pos_ini_clic.y - pos_fin_clic.y);

            float distanciaclicsy = Vector3.Distance(pos_ini_clic, pos_fin_clic);

            float unidadesmovidas = distanciaclicsy / unidad_pantalla_ajustada;

            if (unidadesmovidas > 1.5f)
            {
                unidadesmovidas = 1.5f;
            }


            Vector3 force = new Vector3(mouse.normalized.x, valor_vector_angulo_lanzamiento_base, mouse.normalized.y);

            esfera.AddForce(force * fuerzaActual * 100 * unidadesmovidas);
        }
    }
    IEnumerator activarMovimiento(bool estado, float segundosespera)
    {
        yield return new WaitForSeconds(segundosespera);
        activador_movimiento = estado;
    }
    public void lanzarPorRampa()
    {
        if (mepuedomover)
        {
            esfera.velocity = Vector3.zero;
            Vector3 force = new Vector3(0, 1, 1);
            esfera.AddForce(force.normalized * fuerzaBase * 100 * porcentajeSalto);
            eventosfeel.lanzarrampa();
        }        
    }
    public void actualizarDatos(float fuerza, float porcentajeSaltop, float porcentajereduccionp, float distaniasuelop)
    {
        fuerzaBase = fuerza;
        fuerzaActual = fuerza;

        porcentajeReduccion = porcentajereduccionp;
        porcentajeSalto = porcentajeSaltop;
        distacia_del_suelo = distaniasuelop;

        personaje_material.material = limpio;
    }


    public void chocarcollisonador(Collision collision)
    {
        if (!yachoquemultiplicador)
        {
            yachoquemultiplicador = true;
            ContactPoint contacto = collision.contacts[0];
            GameObject instanciado = Instantiate(eventosfeel.par_monedas_multiplicador, contacto.point, Quaternion.identity) as GameObject;

            eventosfeel.chocarContraMultiplicador();

            controlNivel.monedas_nivel = (controlNivel.monedas_nivel * (collision.gameObject.GetComponent<MultiplicadorIde>().ide));
            controlNivel.txt_monedas_nivel.text = (controljuego.monedas_totales + controlNivel.monedas_nivel).ToString();


            StopAllCoroutines();
            mepuedomover = false;
        }
    }

    public void metodoColisionconpiso(Collision collision)
    {
        if (vibrar_colision)
        {
            vibrar_colision = false;
            eventosfeel.tocarpiso();
            vibration.m_ImpulseDefinition.m_AmplitudeGain = valor_vibracion_piso;
            vibration.GenerateImpulse();


            if (collision != null && nivelMancha > 0 && collision.gameObject.tag.Equals("PisoPiso"))
            {
                ContactPoint contacto = collision.contacts[0];
                GameObject instanciado = Instantiate(eventosfeel.par_mancha, contacto.point, Quaternion.identity) as GameObject;
            }

        }
    }

    public bool estacercadelsuelo()
    {
        bool respuesta = false;

        int mask = (1 << 13) | (1 << 14) | (1 << 16) | (1 << 17) | (1 << 18) | (1 << 19);
        RaycastHit hit;
        var ray = new Ray(transform.position, Vector3.down); ;
        if (Physics.Raycast(ray, out hit, distacia_del_suelo, mask))
        {
            respuesta = true;
        }

        return respuesta;
    }
    public void activarFlecha()
    {
        activarflecha = true;
    }
    public void activarVibracion()
    {
        vibrar_colision = true;
    }
    public void activartocarrampa()
    {
        puedotocarrampa = true;
    }
    public void manchar()
    {
        fuerzaActual = fuerzaBase * porcentajeReduccion;
        splashcamera.SetTrigger("manchar");
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
        }
    }
    public void aumentarPorTocarAgua()
    {
        fuerzaActual = fuerzaBase;
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

        }
    }
    public void caerEnPiscinaMancha()
    {
        fuerzaActual = fuerzaBase * porcentajeReduccion;
        personaje_material.material = nivel3mancha;
        // Textura todo manchado
        nivelMancha = 3;
    }
    public void caerEnPiscinaAgua()
    {
        fuerzaActual = fuerzaBase;
        personaje_material.material = limpio;
        // Textura limpio
        nivelMancha = 0;
        personaje_control.despegarObjetos();
    }
}
