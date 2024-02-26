using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float distanceFromCamera = 2;

    [SerializeField] private float springStiffness = 100;
    [SerializeField] private float dampingRatio = 0.7f;

    [SerializeField] private float hudXOffset = 0;
    [SerializeField] private float hudYOffset = 0;

    private Vector3 targetPosition;
    private Vector3 velocity;

    private void Start()
    {
        ResetPosition();
    }

    private void Update()
    {
        SetHudPosition();
    }

    private void ResetPosition()
    {
       
        transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * distanceFromCamera;
        transform.LookAt(head);

        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    private void SetHudPosition()
    {
        Vector3 targetOffset = head.up * hudYOffset + head.right * hudXOffset;
        targetPosition = head.position + head.forward * distanceFromCamera + targetOffset;

        float deltaTime = Time.deltaTime;
        Vector3 displacement = targetPosition - transform.position;
        Vector3 springForce = springStiffness * displacement;
        Vector3 dampingForce = -2.0f * Mathf.Sqrt(springStiffness) * dampingRatio * velocity;
        Vector3 totalForce = springForce + dampingForce;
        Vector3 acceleration = totalForce / transform.localScale.x;
        velocity += acceleration * deltaTime;
        transform.position += velocity * deltaTime;

        transform.LookAt(head);
        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
        //AudioManager.I.StopBackGroundSounds();
        //AudioManager.I.PlaySceneMusic(SoundName.Level1Music);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
