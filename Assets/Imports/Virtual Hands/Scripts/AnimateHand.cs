using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHand : MonoBehaviour
{
    [SerializeField] private InputActionProperty pinchAction;
    [SerializeField] private InputActionProperty gripAction;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = pinchAction.action.ReadValue<float>();
        float gripValue = gripAction.action.ReadValue<float>();
        animator.SetFloat("Trigger", triggerValue);
        animator.SetFloat("Grip", gripValue);
    }
}
