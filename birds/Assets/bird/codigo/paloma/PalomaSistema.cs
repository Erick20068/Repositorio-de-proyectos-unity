using UnityEngine;
using System.Collections.Generic;

public class PalomasSistema : MonoBehaviour
{
    [Header("Referencias")]
    public Transform referencia; // puede ser la camara main o pude estar vacio 
    public Transform puntoSpawn; // punto específico para el spawn, si no se asigna se usará la referencia o este objeto como origen
    public GameObject palomaPrefab;

    [Header("Parvada")]
    public int cantidadPalomas = 10;
    public float distancia = 20f;
    public float dispersion = 4.5f;
    public float velocidad = 3f;
    public float tiempoEnPantalla = 7f;

    [Header("Escala")]
    public float tiempoEscaladoInicial = 1f;
    public float tiempoEscaladoFinal = 0.8f;
    public Vector3 escalaObjetivo = Vector3.one;

    [Header("Separación")]
    public float distanciaMinimaEntrePalomas = 15f;

    [Header("Animación")]
    public float duracionAnimacionOriginal = 3f;

    private List<GameObject> palomasActuales = new List<GameObject>();
    private List<Vector3> direcciones = new List<Vector3>();

    private float temporizadorPalomas = 0f;
    private bool activo = false;

    // play para crear la parvada, llamado desde afuera cuando se quiera iniciar el efecto
    public void Play()
    {
        if (activo) return;

        CrearParvada();
        temporizadorPalomas = tiempoEnPantalla;
        activo = true;
    }

    // stop para destruir la parvada, llamado desde Update() al finalizar el tiempo o desde afuera si se quiere cancelar antes
    public void Stop()
    {
        DestruirParvada();
        activo = false;
    }

    void Update()
    {
        if (!activo) return;

        temporizadorPalomas -= Time.deltaTime;

        ActualizarEscala();
        MoverPalomas();

        if (temporizadorPalomas <= 0)
        {
            Stop();
        }
    }

    // metodo para crear la parvada, llamado desde Play()
    void CrearParvada()
    {
        // punto de origen y dirección de la parvada, se determina en orden de prioridad: puntoSpawn > referencia > este objeto
        Vector3 origen;
        Vector3 forward;

        if (puntoSpawn != null)
        {
            origen = puntoSpawn.position;
            forward = puntoSpawn.forward;
        }
        else if (referencia != null)
        {
            origen = referencia.position;
            forward = referencia.forward;
        }
        else
        {
            origen = transform.position;
            forward = transform.forward;
        }

        Vector3 centro = origen + Vector3.up * distancia;

        Vector3 direccionParvada = forward;
        direccionParvada.y = 0f;
        direccionParvada.Normalize();

        List<Vector3> posicionesUsadas = new List<Vector3>();

        for (int i = 0; i < cantidadPalomas; i++)
        {
            Vector3 posicionRandom = Vector3.zero;
            bool posicionValida = false;
            int intentos = 0;

            while (!posicionValida && intentos < 20)
            {
                posicionRandom = centro + new Vector3(
                    Random.Range(-dispersion, dispersion),
                    Random.Range(-1f, 1f),
                    Random.Range(-dispersion, dispersion)
                );

                posicionValida = true;

                foreach (Vector3 pos in posicionesUsadas)
                {
                    if (Vector3.Distance(posicionRandom, pos) < distanciaMinimaEntrePalomas)
                    {
                        posicionValida = false;
                        break;
                    }
                }

                intentos++;
            }

            posicionesUsadas.Add(posicionRandom);

            GameObject paloma = Instantiate(
                palomaPrefab,
                posicionRandom,
                Quaternion.LookRotation(direccionParvada)
            );

            //ajustar velocidad de animación para que coincida con el tiempo en pantalla
            Animator anim = paloma.GetComponent<Animator>();
            if (anim != null)
            {
                float velocidadAnim = duracionAnimacionOriginal / tiempoEnPantalla;
                anim.speed = velocidadAnim;
            }

            paloma.transform.localScale = Vector3.zero;

            palomasActuales.Add(paloma);
            direcciones.Add(direccionParvada);
        }
    }

    // escala las palomas desde 0 a su tamaño objetivo durante el tiempo definido, llamado desde Update()
    void ActualizarEscala()
    {
        float tiempoTranscurrido = tiempoEnPantalla - temporizadorPalomas;
        float factorEscala;

        if (tiempoTranscurrido < tiempoEscaladoInicial)
        {
            float t = tiempoTranscurrido / tiempoEscaladoInicial;
            factorEscala = Mathf.SmoothStep(0f, 1f, t);
        }
        else if (temporizadorPalomas < tiempoEscaladoFinal)
        {
            float t = temporizadorPalomas / tiempoEscaladoFinal;
            factorEscala = Mathf.SmoothStep(0f, 1f, t);
        }
        else
        {
            factorEscala = 1f;
        }

        foreach (GameObject paloma in palomasActuales)
        {
            if (paloma != null)
                paloma.transform.localScale = escalaObjetivo * factorEscala;
        }
    }

    //metodo de movimiento, llamado desde Update() mientras la parvada esté activa
    void MoverPalomas()
    {
        for (int i = 0; i < palomasActuales.Count; i++)
        {
            if (palomasActuales[i] != null)
            {
                palomasActuales[i].transform.position +=
                    direcciones[i] * velocidad * Time.deltaTime;
            }
        }
    }

    // metodo para destruir la parvada, llamado desde Stop() o al finalizar el tiempo
    void DestruirParvada()
    {
        foreach (GameObject paloma in palomasActuales)
        {
            if (paloma != null)
                Destroy(paloma);
        }

        palomasActuales.Clear();
        direcciones.Clear();
        temporizadorPalomas = 0f;
    }

    [ContextMenu("Play Palomas prueba")]
    void TestPlay()
    {
        Play();
    }
}