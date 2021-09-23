using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenciaPersonaje : MonoBehaviour
{
    public PersonajeControl personaje;
    public EventosFeel eventosfeel;

    // Particulas especiales
    public GameObject particulasplash;
    private void OnCollisionEnter(Collision collision)
    {
        // Piso
        if (collision.gameObject.layer == 13)
        {
            
            personaje.movimiento_esfera.metodoColisionconpiso(collision);

            // Código para generación de particulas en puntos especificos

            //ContactPoint contacto = collision.contacts[0];

            //GameObject instanciado = Instantiate(particulaespecial1, contacto.point, Quaternion.identity) as GameObject;
        }
        // Piscina Mala
        else if (collision.gameObject.layer == 18)
        {
            eventosfeel.caerpiscinamala();
        }
        // Piscina buena
        else if (collision.gameObject.layer == 19)
        {
            eventosfeel.piscinaagua();
            personaje.despegarObjetos();
            personaje.movimiento_esfera.caerEnPiscinaAgua();
            ContactPoint contacto = collision.contacts[0];
            GameObject instanciado = Instantiate(eventosfeel.par_piscina_agua, contacto.point, Quaternion.identity) as GameObject;
        }
        // Piscina pared
        else if (collision.gameObject.layer == 22)
        {
            eventosfeel.tocarpared();
        }
        // Charco Malo
        else if (collision.gameObject.layer == 16)
        {
            //eventosfeel.tocarliquidomalo();
            //Código para generación de particulas en puntos especificos

            ContactPoint contacto = collision.contacts[0];
            GameObject instanciado = Instantiate(eventosfeel.par_charco_malo, contacto.point, Quaternion.identity) as GameObject;
        }
        // Charco buena
        else if (collision.gameObject.layer == 17)
        {
            eventosfeel.tocaragua();
        }
        // Enemigo
        else if (collision.gameObject.layer == 21)
        {
            eventosfeel.chocarcontraotropersonaje();
        }
        // Multiplicador
        else if (collision.gameObject.layer == 23)
        {
            personaje.movimiento_esfera.chocarcollisonador(collision);
        }
       

    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == 20)
        //{
        //    if (personaje.movimiento_esfera.nivelMancha > 0)
        //    {
        //        if (!personaje.tengoObjetoRegistradoDentro(other.gameObject))
        //        {
        //            other.gameObject.GetComponent<ObjetoEscenario>().crearPesaMala();
        //            personaje.objetosTocadosRegistro.Add(other.gameObject);
        //        }
        //    }
        //    else
        //    {
        //        personaje.movimiento_esfera.aumentarPorObjeto();
        //    }
        //}
    }

    private void Start()
    {
        this.GetComponent<Rigidbody>().maxDepenetrationVelocity = 16f;
    }
}
