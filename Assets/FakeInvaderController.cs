using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeInvaderController : MonoBehaviour
{
    private GameObject fuga;

    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        fuga = GameObject.Find("Fuga");
        speed = Random.Range(10, 50);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(fuga.transform.position);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


}
