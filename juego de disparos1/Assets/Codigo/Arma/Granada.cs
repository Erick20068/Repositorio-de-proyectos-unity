using UnityEngine;

public class Granada : MonoBehaviour
{
    public float delay = 3;

    float countdown;

    public float radius = 5;

    public float explosionForce = 70;

    bool exploded = false;

    public GameObject explosionEffect;



    void Start()
    {
        countdown = delay;

    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0 && exploded == false)
        {
            Exploded();
            exploded = true;

        }

    }

    void Exploded()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);


        //meter en un array los objetos colisionados 
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        //por cada objeto dentro del array cogemos el componente de rigidbody
        foreach (var rangeObjects in colliders)
        {
            Rigidbody rb = rangeObjects.GetComponent<Rigidbody>();

            //En caso de ser verdad le afectara la fuerza de impulso 
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce * 10, transform.position,radius);
            }

        }

        Destroy(gameObject);

    }

 


}
