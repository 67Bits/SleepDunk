using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class EventosFeel : MonoBehaviour
{
    public MMFeedbacks vfx_halar, vfx_salto, vfx_tocarpiso, vfx_tocarpared, vfx_tocarliquidomalo,
        vfx_tocaragua, vfx_lanzarrampa, vfx_chocarcontraotropersonaje, vfx_caerpiscinamala, vfx_piscinaagua, vfx_tocarobjetomalo,
        vfx_dormiralllegarameta, vfx_salirmonedas, vfx_chocarContraMultiplicador;
    public GameObject par_charco_malo, par_piscina_agua, par_monedas_multiplicador, par_confetis, par_rampa, par_mancha;
    // Hecho
    public void halar()
    {
        vfx_halar.PlayFeedbacks();
    }
    // Hecho
    public void salto()
    {
        vfx_salto.PlayFeedbacks();
    }
    // Hecho
    public void tocarpiso()
    {
        vfx_tocarpiso.PlayFeedbacks();
    }
    // Hecho
    public void tocarpared()
    {
        vfx_tocarpared.PlayFeedbacks();
    }
    // Hecho
    public void tocarliquidomalo()
    {
        vfx_tocarliquidomalo.PlayFeedbacks();
    }
    // Hecho
    public void tocaragua()
    {
        vfx_tocaragua.PlayFeedbacks();
    }
    // Hecho
    public void lanzarrampa()
    {
        vfx_lanzarrampa.PlayFeedbacks();
    }
    // Hecho
    public void chocarcontraotropersonaje()
    {
        vfx_chocarcontraotropersonaje.PlayFeedbacks();
    }
    // Hecho
    public void caerpiscinamala()
    {
        vfx_caerpiscinamala.PlayFeedbacks();
    }
    // Hecho
    public void piscinaagua()
    {
        vfx_piscinaagua.PlayFeedbacks();
    }
    // Hecho
    public void tocarobjetomalo()
    {
        vfx_tocarobjetomalo.PlayFeedbacks();
    }

    // Hecho
    public void dormirallelgaralameta()
    {
        vfx_dormiralllegarameta.PlayFeedbacks();
    }

    // Hecho
    public void generarMonedas()
    {
        vfx_salirmonedas.PlayFeedbacks();
    }

    // Hecho
    public void chocarContraMultiplicador()
    {
        vfx_chocarContraMultiplicador.PlayFeedbacks();
    }

}
