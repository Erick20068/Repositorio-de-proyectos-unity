using UnityEngine;
using System.Collections.Generic;

public class HologramaEscala : MonoBehaviour
{
    [Header("Meshes del Holograma")]
    public List<Renderer> renderers = new List<Renderer>();

    [Header("Parámetros de Animación")]
    public float velocidad = 0.01f;
    public float tilingBase = 1f;

    [Header("Dirección")]
    public bool moverEnY = false;   // true = vertical, false = horizontal
    public bool invertir = false;   // invierte dirección

    [Header("Estado")]
    [SerializeField] private EstadoAnimacion estado = EstadoAnimacion.Detenido;

    public enum EstadoAnimacion { Detenido, Reproduciendo }

    private float tiempoAcumulado = 0f;


    void Start()
    {
        // Si no se asignaron renderers manualmente, intenta obtenerlos automáticamente
        if (renderers.Count == 0)
            renderers.AddRange(GetComponentsInChildren<Renderer>());

        Play();
    }

    //mueve la textura hacia arriba y ajusta el tiling según la escala del renderer

    void Update()
    {
        if (estado != EstadoAnimacion.Reproduciendo) return;

        AnimarTextura(); 
    }

    void AnimarTextura()
    {
        tiempoAcumulado += Time.deltaTime;

        float direccion = invertir ? -1f : 1f;
        float offset = tiempoAcumulado * velocidad * direccion;

        foreach (Renderer rend in renderers)
        {
            if (rend == null) continue;

            // Calcula tiling según la escala del renderer, no del padre
            Vector3 escala = rend.transform.lossyScale;
            float promedio = (escala.x + escala.y + escala.z) / 3f;
            float tiling = tilingBase * promedio;

            if (moverEnY)
                rend.material.SetTextureOffset("_BaseMap", new Vector2(0, offset));
            else
                rend.material.SetTextureOffset("_BaseMap", new Vector2(offset, 0));

            rend.material.mainTextureScale = new Vector2(tiling, tiling);
        }
    }

    // Inicia la animación
    public void Play()
    {
        estado = EstadoAnimacion.Reproduciendo;
    }

    // Detiene la animación y resetea el tiempo acumulado
    public void Stop()
    {
        estado = EstadoAnimacion.Detenido;
        tiempoAcumulado = 0f;

        foreach (Renderer rend in renderers)
        {
            if (rend == null) continue;
            rend.material.mainTextureOffset = Vector2.zero;
        }
    }

    // Pausa la animación sin resetear el tiempo acumulado
    public void TogglePlayPause()
    {
        if (estado == EstadoAnimacion.Reproduciendo) Stop();
        else Play();
    }

    // Métodos para el editor, permiten controlar la animación desde el inspector

#if UNITY_EDITOR
    [ContextMenu("▶ Play")] void EditorPlay() => Play();
    [ContextMenu("■ Stop")] void EditorStop() => Stop();
#endif
}