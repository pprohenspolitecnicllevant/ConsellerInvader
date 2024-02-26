using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    [SerializeField] private Transform rightAttach;
    [SerializeField] private Transform leftAttach;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.TryGetComponent(out RightHand _))
            attachTransform = rightAttach;
        else if (args.interactorObject.transform.TryGetComponent(out LeftHand _))
            attachTransform = leftAttach;

        base.OnSelectEntered(args);
    }
}
