using UnityEngine;

public class Bala : MonoBehaviour
{

    // cuando la bala colisione con algo va a guardar en el objeto de collision 
    private void OnCollisionEnter(Collision collision)
    {
        //Pregunatmos si hemos colisionado con un objeto con el tag(etiqueta) Enemigo
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Destroy(collision.gameObject);
        }



    }


}

