using UnityEngine;
using UnityEngine.UIElements;

public class TirarGranada : MonoBehaviour
{

    public float throwForce = 500;

    public GameObject granadaPrefab;
    

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            Throw();
        }
        
    }

    public void Throw()
    {
        GameObject newGranada = Instantiate(granadaPrefab,transform.position,transform.rotation);

        newGranada.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
    }
}
