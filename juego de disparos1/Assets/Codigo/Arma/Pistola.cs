using UnityEngine;
using UnityEngine.InputSystem;

public class Pistola : MonoBehaviour
{
    // Punto desde donde salen las balas
    public Transform spawnPoint;

    // Prefab de la bala
    public GameObject bullet;

    // Fuerza con la que se dispara la bala
    public float shortForce = 1500f;

    // Tiempo entre disparos (cadencia)
    public float shortRate = 0.5f;

    // Controla el tiempo del último disparo
    private float shortRateTime = 0;



    void Update()
    {
        // Detecta clic izquierdo del mouse (Fire1)
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            if(Time.time>shortRateTime && GameManager.Instance.gunAmmo > 0)
            {

                GameManager.Instance.gunAmmo--;

                // Estancia de una bala 
                GameObject newBullet;

                // Crea (instancia) una nueva bala en la posición del spawnPoint
                newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

                newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * shortForce);


                // Es el tiempo de disparo de la bala 
                shortRateTime = Time.time + shortRate;

                //Destruir la bala despues de 5 segundos 
                Destroy(newBullet,5);
            }
            
        }
    }
}