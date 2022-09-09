using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallForces : MonoBehaviour
{
    public enum BolitaRunMode
    {
        Friction,
        FluidFriction,
        //Gravity
    }



    private MyVector2D displacement;
    [SerializeField] private BolitaRunMode runMode; 
    [SerializeField] Camera camera;
    MyVector2D ballPosition;
    MyVector2D velocity;
    MyVector2D acceleration;

    [Header("Forces")]
    [SerializeField] float mass;
    private  MyVector2D weight;
    //[SerializeField] MyVector2D wind;
    [SerializeField] MyVector2D gravity;
    private MyVector2D netforce;

    [Header("Frictions")]
    [Range(0f, 1f)] [SerializeField] private float frictionCoeficient;
    [SerializeField] private bool useFluidFriction = false; 
    [Range(0f, 1f)] [SerializeField] float dampingFactor = 0.9f;


    private void Start()
    {
        ballPosition = new MyVector2D(transform.position.x, transform.position.y);
    }

    private void FixedUpdate() // para garantizar un deltatime constante 
    {
        
        //ApplyForce(wind);

        if (runMode == BolitaRunMode.FluidFriction)
        {
            netforce = new MyVector2D(0, 0); // la suma de todas las fuerzas que interactuan con el objeto
            weight = gravity * mass;
            ApplyForce(weight);
            Debug.Log("fluido");
            FluidFriction();
        }
        else if (runMode == BolitaRunMode.Friction)
        {
            netforce = new MyVector2D(0, 0); // la suma de todas las fuerzas que interactuan con el objeto
            weight = gravity * mass;
            ApplyForce(weight);
            ApplyForce(Friction());
        }
        /*else if (runMode == BolitaRunMode.Gravity)
        {
            MyVector2D diff = ballPosition - displacement;
            float distance = diff.magnitud;
            float scalarPart = (mass * mass) / (distance * distance);
            MyVector2D gravity = scalarPart * diff.normalized;

            ApplyForce(gravity); 
        }*/
 
        Move();
    }


    private void Update()
    {
        //displacement.Draw(ballPosition, Color.red);
        ballPosition.Draw(Color.blue);
        //velocity.Draw(ballPosition, Color.cyan);
        acceleration.Draw(ballPosition, Color.red);
    }

    public void Move() // METODO PARA QUE SE MUEVA LA BOLITA
    {
        // la integral de la aceleracion es la velocidad 
        // la integral de la velocidad es la posicion 

        velocity = velocity + acceleration * Time.fixedDeltaTime;
        displacement = Time.fixedDeltaTime * velocity;
        ballPosition = ballPosition + displacement;

        Debug.Log("The ball is moving");

        //horizontal bounce check

        if (Mathf.Abs(ballPosition.x) > camera.orthographicSize)
        {
            velocity.x = velocity.x * -1;
            ballPosition.x = Mathf.Sign(ballPosition.x) * camera.orthographicSize;
            velocity *= dampingFactor; // damping factor
        }

        //Vertical bounce check

        if (Mathf.Abs(ballPosition.y) > camera.orthographicSize)
        {
            velocity.y = velocity.y * -1;
            ballPosition.y = Mathf.Sign(ballPosition.y) * camera.orthographicSize;
            velocity *= dampingFactor; // damping factor
        }

        transform.position = new Vector3(ballPosition.x, ballPosition.y, 0);
    }


    public void ApplyForce(MyVector2D appliedForce) // CALCULO DE FUERZAS APLICADAS AL OBJETO
    {
        netforce += appliedForce;
        acceleration = netforce / mass;
    }

    private MyVector2D Friction() //CALCULO DE FRICCIÓN
    {
        float normalForce = gravity.magnitud * mass; 
        MyVector2D resultFriction = -frictionCoeficient * normalForce * velocity.normalized;

        return resultFriction;
    }

    private MyVector2D FluidFriction()
    {
        if (transform.localPosition.y <= 0)
        {
            float frontalArea = transform.localScale.x;
            float rho = 1; //densidad 
            float fluidDragCoefficient = 1;
            float velocityMagnitude = velocity.magnitud;
            float scalarPart = -0.5f * rho * velocityMagnitude * frontalArea * fluidDragCoefficient;

            MyVector2D fluidFriction = scalarPart * velocity.normalized;
            fluidFriction.Draw(ballPosition, Color.cyan);
            ApplyForce(fluidFriction);

        }

        return new MyVector2D();
    }
}





