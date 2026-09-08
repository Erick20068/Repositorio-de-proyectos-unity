using UnityEngine;
using System.Collections;

public class RayoControl : MonoBehaviour
{
    public Transform origen;
    public Transform destino;
    public ParticleSystem particulas;

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    void Update()
    {
        if (lr.enabled)
        {
            lr.SetPosition(0, origen.position);
            lr.SetPosition(1, destino.position);

            //anima la textura del rayo para dar sensación de movimiento
            float speed = 2f;
            lr.material.mainTextureOffset = new Vector2(Time.time * speed, 0);
        }
    }
    public void Play()
    {
        lr.enabled = true;

        //reproduce las partículas al inicio del rayo
        if (particulas != null)
            particulas.Play();

        StartCoroutine(AnimarRayo());
    }

    //anima el rayo desde el origen al destino
    IEnumerator AnimarRayo()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 2;


            //interpola la posición del extremo del rayo desde el origen al destino
            Vector3 punto = Vector3.Lerp(origen.position, destino.position, t);
            lr.SetPosition(1, punto);

            yield return null;
        }
    }

    //detiene el rayo y las partículas, luego hace un fade out del rayo
    public void Stop()
    {
        if (particulas != null)
            particulas.Stop();

        StartCoroutine(FadeOut());
    }

    //hace un fade out del rayo disminuyendo su alpha hasta desaparecer
    IEnumerator FadeOut()
    {
        float t = 1;
        Color c = lr.material.color;

        while (t > 0)
        {
            t -= Time.deltaTime;
            c.a = t;
            lr.material.color = c;

            yield return null;
        }

        lr.enabled = false;
    }

#if UNITY_EDITOR
    [ContextMenu("▶ Play")]
    void EditorPlay() => Play();

    [ContextMenu("■ Stop")]
    void EditorStop() => Stop();
#endif
}