using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public MeshRenderer Renderer;

    // Variáveis para movimento suave
    public Vector3 targetPosition;
    public float moveSpeed = 1.0f;

    // Variáveis para mudança gradual de escala
    public float minScale = 1.0f;
    public float maxScale = 3.0f;
    public float scaleChangeSpeed = 0.5f;
    private bool increasingScale = true;

    // Variáveis para rotação em todos os eixos
    public Vector3 rotationSpeed;

    // Variáveis para velocidade de rotação variável
    public float minRotationSpeed = 5.0f;
    public float maxRotationSpeed = 20.0f;

    // Variáveis para alteração aleatória de cor
    public float colorChangeInterval = 2.0f;
    private float lastColorChangeTime;

    // Variáveis para piscar de opacidade
    public float minOpacity = 0.2f;
    public float maxOpacity = 1.0f;
    public float opacityChangeSpeed = 1.0f;
    private bool increasingOpacity = true;

    void Start()
    {
        // Definir a posição inicial aleatória
        targetPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));

        // Definir a rotação inicial aleatória
        rotationSpeed = new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f), Random.Range(-30f, 30f));

        // Definir o tempo da última mudança de cor
        lastColorChangeTime = Time.time;

        // Definir a cor inicial aleatória
        Material material = Renderer.material;
        material.color = new Color(Random.value, Random.value, Random.value);

        // Definir a opacidade inicial aleatória
        Color color = material.color;
        color.a = Random.Range(minOpacity, maxOpacity);
        material.color = color;
    }

    void Update()
    {
        // Movimento suave
        targetPosition = new Vector3(Mathf.PingPong(Time.time, 6) - 3, Mathf.PingPong(Time.time, 6) - 5, Mathf.PingPong(Time.time, 6) - 3);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Rotação em todos os eixos
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // Velocidade de rotação variável
        float currentRotationSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, Mathf.PingPong(Time.time, 1));
        transform.Rotate(Vector3.one * currentRotationSpeed * Time.deltaTime);

        // Alteração aleatória de cor
        if (Time.time - lastColorChangeTime > colorChangeInterval)
        {
            Material material = Renderer.material;
            material.color = new Color(Random.value, Random.value, Random.value);
            lastColorChangeTime = Time.time;
        }

        // Piscar de opacidade
        Material cubeMaterial = Renderer.material;
        float currentOpacity = cubeMaterial.color.a;
        if (increasingOpacity)
        {
            currentOpacity += opacityChangeSpeed * Time.deltaTime;
            if (currentOpacity >= maxOpacity)
                increasingOpacity = false;
        }
        else
        {
            currentOpacity -= opacityChangeSpeed * Time.deltaTime;
            if (currentOpacity <= minOpacity)
                increasingOpacity = true;
        }
        cubeMaterial.color = new Color(cubeMaterial.color.r, cubeMaterial.color.g, cubeMaterial.color.b, currentOpacity);

        // Mudança gradual de escala
        float targetScale = increasingScale ? maxScale : minScale;
        float currentScale = transform.localScale.x;
        currentScale = Mathf.MoveTowards(currentScale, targetScale, scaleChangeSpeed * Time.deltaTime);
        transform.localScale = Vector3.one * currentScale;

        // Inverter direção da mudança de escala quando atingir o limite
        if (Mathf.Approximately(currentScale, maxScale) || Mathf.Approximately(currentScale, minScale))
            increasingScale = !increasingScale;
    }
}
