    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class IniciadorFeels : MonoBehaviour
{
    public MMFeedbacks plataforma;
    void Start()
    {
        Invoke("activar", 0.2f);
    }
    public void activar()
    {
        plataforma.PlayFeedbacks();
    }
}
