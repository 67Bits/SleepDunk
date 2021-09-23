using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoEscenario : MonoBehaviour
{

    private MeshRenderer miMesh;
    public GameObject prefab_pesa;


    // Start is called before the first frame update
    void Start()
    {
        miMesh = gameObject.GetComponent<MeshRenderer>();
        miMesh.enabled = true;
    }

    public void crearPesaMala()
    {
        GameObject instanciado = Instantiate(prefab_pesa);
        instanciado.transform.parent = gameObject.transform;
        instanciado.transform.localPosition = Vector3.zero;
        miMesh.enabled = false;
        Invoke("reaparecer", 3);
    }
    public void reaparecer()
    {
        miMesh.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
