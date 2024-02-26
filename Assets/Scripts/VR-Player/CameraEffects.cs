using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.5f; // Duración del temblor en segundos
    [SerializeField] private float shakeAmplitude = 0.1f; // Magnitud del temblor
    [SerializeField] private float shakeFrequency = 1.0f; // Frecuencia del temblor

    private float shakeElapsedTime = 0f;
    private Vector3 originalPos;

    private void OnEnable()
    {
        PlayerHealthController.HealthModified += ShakeCamera;
    }

    private void OnDisable()
    {
        PlayerHealthController.HealthModified -= ShakeCamera;
    }
    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeElapsedTime > 0)
        {
            // Genera un valor de ruido aleatorio
            float perlinNoise = Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) * 2f - 1f;

            // Aplica el valor de ruido a la posición de la cámara
            Vector3 pos = originalPos + new Vector3(perlinNoise, perlinNoise, 0f) * shakeAmplitude;

            transform.localPosition = pos;

            // Disminuye la duración del temblor con el tiempo
            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            // Detiene el temblor
            transform.localPosition = originalPos;
            shakeElapsedTime = 0f;
        }
    }


    public void ShakeCamera(int amount)
    {
        // Inicia el temblor de la cámara
        shakeElapsedTime = shakeDuration;
    }
}
