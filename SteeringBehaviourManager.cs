/**
 * An easy to use Steering Behaviours Manager. You have to add this component to a Game Object.
 * Author: Javier (Delunado).
 * 
 * Last Update: 11/6/2021.
 * - First public version
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviourManager : MonoBehaviour
{
    [Tooltip("The max force the steering will have.")]
    [SerializeField] float maxForce = 3.0f;
    [Tooltip("The mass used in the steering calculus. A higher mass implies a slower movement.")]
    [SerializeField] float mass = 1.0f;
    [Tooltip("The radius from which the Arrival behaviour will start to get slower.")]
    [SerializeField] float slowRadius = 2.0f;

    [SerializeField] float wanderCircleDistance = 1.0f;
    [SerializeField] float wanderCircleRadius = 1.0f;
    [SerializeField] float wanderChangeAngle = 5.0f;

    private float wanderAngle;

    private Vector2 actualSteering;

    /// <summary>
    /// Initialize the Manager
    /// </summary>
    private void Start()
    {
        wanderAngle = Random.Range(0.0f, 360.0f);

        actualSteering = Vector2.zero;
    }

    /// <summary>
    /// Returns the calculated final force. This is the final step after applying the desired behaviours (Seek, Flee, etc.).
    /// </summary>
    /// <returns></returns>
    public Vector2 GetForce()
    {
        Vector2 finalSteering = ProcessSteering(actualSteering);

        Reset();

        return finalSteering;
    }

    /// <summary>
    /// Resets the calculated force. You don't need to call this if you use GetForce().
    /// </summary>
    public void Reset()
    {
        actualSteering = Vector2.zero;
    }

    /*-------------- PUBLIC METHODS ------------------------*/

    /// <summary>
    /// Goes to a certain point.
    /// </summary>
    /// <param name="fromPoint">The point from where you want to move</param>
    /// <param name="targetPoint">The point you want to reach</param>
    /// <param name="actualVelocity">The actual velocity of the entity that will have the SB applied</param>
    /// <param name="speed">The speed of the entity that will have the SB applied</param>
    /// <param name="weight">The weight of the behaviour. Higher means this behaviour will have more importance in the calculus of the steering</param>
    public void Seek(Vector2 fromPoint, Vector2 targetPoint, Vector2 actualVelocity, float speed, float weight = 1.0f)
    {
        actualSteering += ProcessSeek(fromPoint, targetPoint, actualVelocity, speed) * weight;
    }

    /// <summary>
    /// Flees from a certain point.
    /// </summary>
    /// <param name="fromPoint">The point from where you want to move</param>
    /// <param name="targetPoint">The point you want to flee from</param>
    /// <param name="actualVelocity">The actual velocity of the entity that will have the SB applied</param>
    /// <param name="speed">The speed of the entity that will have the SB applied</param>
    /// <param name="weight">The weight of the behaviour. Higher means this behaviour will have more importance in the calculus of the steering</param>
    public void Flee(Vector2 fromPoint, Vector2 targetPoint, Vector2 actualVelocity, float speed, float weight = 1.0f)
    {
        actualSteering += ProcessFlee(fromPoint, targetPoint, actualVelocity, speed) * weight;
    }

    /// <summary>
    /// Goes to a certain point, getting slower when it's reaching the point at certain range.
    /// </summary>
    /// <param name="fromPoint">The point from where you want to move</param>
    /// <param name="targetPoint">The point you want to reach</param>
    /// <param name="actualVelocity">The actual velocity of the entity that will have the SB applied</param>
    /// <param name="speed">The speed of the entity that will have the SB applied</param>
    /// <param name="weight">The weight of the behaviour. Higher means this behaviour will have more importance in the calculus of the steering</param>
    public void Arrival(Vector2 fromPoint, Vector2 targetPoint, Vector2 actualVelocity, float speed, float weight = 1.0f)
    {
        actualSteering += ProcessArrival(fromPoint, targetPoint, actualVelocity, speed) * weight;
    }

    /// <summary>
    /// Avoids obstacles in a certain layer.
    /// </summary>
    /// <param name="fromPoint">The point from where you want to move</param>
    /// <param name="actualVelocity">The actual velocity of the entity that will have the SB applied</param>
    /// <param name="obstacleDetectionLenght">How far the obstacle will be detected</param>
    /// <param name="obstacleMask">The obstacle(s) to be detected layermask</param>
    /// <param name="weight">The weight of the behaviour. Higher means this behaviour will have more importance in the calculus of the steering</param>
    public void ObstacleAvoid(Vector2 fromPoint, Vector2 actualVelocity, float obstacleDetectionLenght, LayerMask obstacleMask, float weight = 1.0f)
    {
        actualSteering += ProcessObstacleAvoid(fromPoint, actualVelocity, obstacleDetectionLenght, obstacleMask) * weight;
    }

    /// <summary>
    /// Wanders around the surface.
    /// </summary>
    /// <param name="fromPoint">The point from where you want to move</param>
    /// <param name="actualVelocity">The actual velocity of the entity that will have the SB applied</param>
    /// <param name="speed">The speed of the entity that will have the SB applied</param>
    /// <param name="weight">The weight of the behaviour. Higher means this behaviour will have more importance in the calculus of the steering</param>
    public void Wandering(Vector2 fromPoint, Vector2 actualVelocity, float speed, float weight = 1.0f)
    {
        actualSteering += WanderForce(fromPoint, actualVelocity, speed) * weight;
    }

    /*--------------------------------------*/

    private Vector2 ProcessSeek(Vector2 fromPoint, Vector2 targetPoint, Vector2 actualVelocity, float speed)
    {
        Vector2 desiredVel = (targetPoint - fromPoint).normalized * speed;

        Vector2 steering = (desiredVel - actualVelocity);

        return steering;
    }

    private Vector2 ProcessFlee(Vector2 fromPoint, Vector2 targetPoint, Vector2 actualVelocity, float speed)
    {
        return ProcessSeek(fromPoint, targetPoint, actualVelocity, speed) * -1;
    }

    private Vector2 ProcessArrival(Vector2 fromPoint, Vector2 targetPoint, Vector2 actualVelocity, float speed)
    {
        Vector2 desiredVel = (targetPoint - fromPoint).normalized * speed;

        float distance = Vector2.Distance(targetPoint, fromPoint);

        if (distance < slowRadius)
            desiredVel *= (distance / slowRadius);

        Vector2 steering = (desiredVel - actualVelocity);

        return steering;
    }


    private Vector2 ProcessSteering(Vector2 steering)
    {
        Vector2 newSteering = Vector2.ClampMagnitude(steering, maxForce);
        newSteering /= mass;

        return newSteering;
    }

    private Vector2 WanderForce(Vector2 fromPoint, Vector2 actualVelocity, float speed)
    {
        Vector2 circleCenter = actualVelocity.normalized * wanderCircleDistance;

        Vector2 displacement = new Vector2(0, -1.0f) * wanderCircleRadius;

        //Random angle
        float length = displacement.magnitude;

        displacement.x = Mathf.Cos(wanderAngle) * length;
        displacement.y = Mathf.Sin(wanderAngle) * length;

        wanderAngle += (Random.value * wanderChangeAngle) - (wanderChangeAngle * 0.5f);

        Vector2 wanderForce = circleCenter + displacement + fromPoint;

        return ProcessSeek(fromPoint, wanderForce, actualVelocity, speed);
    }

    private Vector2 ProcessObstacleAvoid(Vector2 fromPoint, Vector2 actualVelocity, float obstacleDetectionLenght, LayerMask obstacleMask)
    {
        Vector2 rayDir = actualVelocity.normalized;

        Collider2D[] rayhits = Physics2D.OverlapCircleAll(fromPoint, obstacleDetectionLenght, obstacleMask);

        Vector2 steeringFinal = Vector2.zero;
        bool movement = false;

        foreach (Collider2D c in rayhits)
        {
            movement = true;
            float distance = Vector2.Distance(c.transform.position, fromPoint);
            float force = (1.0f / (Mathf.Pow(distance, 2) + 1));

            steeringFinal += ProcessFlee(fromPoint, c.transform.position, actualVelocity, 5.0f) * force;
        }

        if (movement)
            wanderAngle = Mathf.Atan2(steeringFinal.y, steeringFinal.x) * Mathf.Rad2Deg;

        return steeringFinal;
    }
}