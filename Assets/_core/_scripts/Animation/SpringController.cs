using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    public Transform Slot;
    public Animation springAnimation;
    public Transform innerSpring;

    [Range(-90, 90)]
    public float angle;

    [Range(-1, 1)]
    public float t;

    public bool Released = false;

    public float Elasticity = 400.0f;
    public float Friction = 1.5f;

    private float velocity = 0;

    void Start()
    {
        springAnimation["Base"].normalizedTime = 0.5f;
        springAnimation["Base"].speed = 0.0f;
        springAnimation.Play("Base");
    }

    void Update()
    {
        if (Released)
        {
            //// Simulate elastic movement
            // F = -K * dx
            float elasticForce = -Elasticity * t;

            // F = m * a; we approximate mass to 1 and work on Elasticity to model the graphics feedback
            // F -> 1 * a
            float acceleration = elasticForce - velocity * Friction;

            // V = a * t
            velocity += acceleration * Time.deltaTime;

            // Integrates changes between last timestep
            // h += V * t + 0.5 * a * t^2
            t += velocity * Time.deltaTime + 0.5f * acceleration * Time.deltaTime * Time.deltaTime;
        }
        else
        {
            velocity = 0;

            innerSpring.transform.localRotation = Quaternion.AngleAxis(180 + Mathf.Clamp(angle, -90, 90), Vector3.up);
        }

        t = Mathf.Clamp(t, -1, 1);

        springAnimation["Base"].normalizedTime = (t + 1) * 0.5f;
    }
}
