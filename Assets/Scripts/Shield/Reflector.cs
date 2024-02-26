using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    private Rigidbody lastRigidBody = null;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InvaderBullet") && other.gameObject.TryGetComponent(out Rigidbody rb))
        {
            if (rb == lastRigidBody)
                return;

            lastRigidBody = rb;

            Vector3 point = other.ClosestPoint(rb.position);

            Vector3 normal= (rb.position - point).normalized;

            Vector3 reflectVelocity = Vector3.Reflect(rb.velocity, normal);

            

            Vector3 direction = reflectVelocity.normalized;
            float angle = Random.Range(-15f, 15f);
            Vector3 randomAxis = Vector3.Cross(direction, Vector3.up).normalized;
            Quaternion rotation = Quaternion.AngleAxis(angle, randomAxis);
            Vector3 newDirection = rotation * direction;

            float speed = reflectVelocity.magnitude;
            Vector3 newVelocity = newDirection * speed;

            rb.rotation = Quaternion.LookRotation(newVelocity.normalized);

            rb.AddForce(newVelocity * 2, ForceMode.VelocityChange);
        }
    }
}
