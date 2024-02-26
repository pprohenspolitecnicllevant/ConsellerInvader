using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float distanceFromCamera = 2;

    [SerializeField] private float springStiffness = 100;
    [SerializeField] private float dampingRatio = 0.7f;

    [SerializeField] private float hudXOffset = 0;
    [SerializeField] private float hudYOffset = 0;


    public void ResetPosition()
    {
        //Vector3 targetOffset = head.up * hudYOffset + head.right * hudXOffset;
        //targetPosition = head.position + head.forward * distanceFromCamera + targetOffset;
        //transform.position = targetPosition;

        transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * distanceFromCamera;
        transform.LookAt(head);

        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        //AudioManager.I.StopBackGroundSounds();
        //AudioManager.I.PlaySceneMusic(SoundName.MainMenuMusic);
    }
}
