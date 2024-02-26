using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWelcomeCanvas : MonoBehaviour
{
    [SerializeField] private GameObject welcomeCanvas;

    public void HideObject()
    {
        if (welcomeCanvas.activeInHierarchy)
            welcomeCanvas.SetActive(false);    
    }
}
