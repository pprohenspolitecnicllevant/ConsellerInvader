using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManager : MonoBehaviour {
    public Animator anim;
    private int state = 0;
    private int maxState = 5;

    public void Start () {
        anim.Play("1 Intro");
    }

    public void NextState() {
        Debug.Log("BUTTON PRESSED");
        if (state < maxState) {
            state++;
        } else {
            Debug.Log("tuto done");
            state = 0;
        }

        switch (state) {
            case 0:
                anim.Play("1 Intro");
                break;
            
            case 1:
                anim.Play("2 Basics");
                break;

            case 2:
                anim.Play("3 Grab");
                break;

            case 3:
                anim.Play("4 Shoot");
                break;

            case 4:
                anim.Play("5 ShieldCharge");
                break;

            case 5:
                anim.Play("6 ShieldRay");
                break;
        }
    }
}
