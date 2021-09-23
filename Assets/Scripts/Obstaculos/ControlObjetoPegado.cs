using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlObjetoPegado : MonoBehaviour
{
    private CharacterJoint joint = null;
    public PersonajeControl personaje_control;
    public EnemigoControl personaje_control2;
    public EventosFeel eventosfeel;

    public bool yamepegue = false;

    // Start is called before the first frame update
    void Start()
    {
        eventosfeel = GameObject.FindGameObjectWithTag("Efectos").GetComponent<EventosFeel>();
        this.GetComponent<Rigidbody>().maxDepenetrationVelocity = 16f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (joint == null)
        {
            if (!yamepegue)
            {
                if (collision.gameObject.layer == 15)
                {
                    if (collision.gameObject.tag.Equals("Player"))
                    {
                        personaje_control = collision.gameObject.GetComponent<ReferenciaPersonaje>().personaje;
                        if (personaje_control.movimiento_esfera.nivelMancha > 0)
                        {
                            eventosfeel.tocarobjetomalo();
                            yamepegue = true;
                            //gameObject.layer = 15;

                            personaje_control.objetospegados.Add(gameObject);

                            joint = gameObject.AddComponent<CharacterJoint>();

                            SoftJointLimit limite = new SoftJointLimit();
                            limite.limit = 177;
                            limite.bounciness = 0;
                            limite.contactDistance = 0;

                            limite.contactDistance = 0;
                            joint.highTwistLimit = limite;
                            joint.lowTwistLimit = limite;
                            joint.lowTwistLimit = limite;

                            joint.swing1Limit = limite;
                            joint.swing2Limit = limite;

                            SoftJointLimitSpring a = new SoftJointLimitSpring();
                            a.damper = 0;

                            joint.swingLimitSpring = a;
                            joint.twistLimitSpring = a;
                            //joint.swingAxis = a;



                            ContactPoint contacto = collision.contacts[0];

                            Vector3 contactoReal = gameObject.transform.position - contacto.point;

                            joint.anchor = -contactoReal;
                            joint.autoConfigureConnectedAnchor = true;

                            joint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
                        }
                    }

                }
                else if (collision.gameObject.layer == 21)
                {
                    if (collision.gameObject.tag.Equals("Enemy"))
                    {
                        personaje_control2 = collision.gameObject.GetComponent<ReferenciadorEnemigo>().personaje;

                        if (personaje_control2.movimiento_esfera.nivelMancha > 0)
                        {
                            yamepegue = true;
                            //gameObject.layer = 15;

                            personaje_control2.objetospegados.Add(gameObject);

                            joint = gameObject.AddComponent<CharacterJoint>();

                            SoftJointLimit limite = new SoftJointLimit();
                            limite.limit = 0;
                            limite.bounciness = 0;
                            limite.contactDistance = 0;

                            limite.contactDistance = 0;
                            joint.highTwistLimit = limite;
                            joint.lowTwistLimit = limite;
                            joint.lowTwistLimit = limite;

                            joint.swing1Limit = limite;
                            joint.swing2Limit = limite;

                            SoftJointLimitSpring a = new SoftJointLimitSpring();
                            a.damper = 0;

                            joint.swingLimitSpring = a;
                            joint.twistLimitSpring = a;
                            //joint.swingAxis = a;

                            joint.enableCollision = false;
                            joint.enablePreprocessing = false;
                            joint.enableProjection = true;

                            ContactPoint contacto = collision.contacts[0];

                            Vector3 contactoReal = gameObject.transform.position - contacto.point;

                            joint.anchor = -contactoReal;
                            joint.autoConfigureConnectedAnchor = true;

                            joint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
                        }
                    }
                }
            }

        }

    }
}
