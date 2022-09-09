using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    [SerializeField] MyVector2D acceleration;
    [SerializeField] MyVector2D velocity;
    [SerializeField] private MyVector2D initialPosition;

    [Header("Planet info: ")]
    [SerializeField] float mass = 1f;
    [SerializeField] Gravity otherPlanet;

    void Start()
    {
        initialPosition = new MyVector2D(transform.position.x, transform.position.y);
    }

    private void FixedUpdate()
    {
  
        acceleration = new MyVector2D(0, 0); 

        ApplyForce(AttractionForce());
        Move();
    }


    void Update()
    {
        initialPosition.Draw(Color.red);
        velocity.Draw(initialPosition, Color.cyan); 
        acceleration.Draw(initialPosition, Color.green);
    }

    public void Move()
    {

        velocity = velocity + acceleration * Time.fixedDeltaTime;
        initialPosition = initialPosition + velocity * Time.fixedDeltaTime; 

        if (velocity.magnitud >= 10f)
        {
            velocity.Normalize();
            velocity *= 10;
        }
        transform.position = new Vector3(initialPosition.x, initialPosition.y);
    }

    private void ApplyForce(MyVector2D force)
    {
        acceleration += force / mass;
    }

    private MyVector2D AttractionForce()
    {
        MyVector2D r = otherPlanet.initialPosition - initialPosition; 
        float rMagnitude = r.magnitud;
        MyVector2D f = r.normalized * (otherPlanet.mass * mass / rMagnitude * rMagnitude); 
        return f;
    }
}
