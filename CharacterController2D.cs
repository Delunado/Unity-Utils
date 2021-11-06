using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    //This is the skin width. We use it to not raycast directly from the sides, but from a little inside.
    const float skinWidth = 0.015f;

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public int horzCollsNumber;

        public void Reset()
        {
            horzCollsNumber = 0;
            above = false;
            below = false;
            left = false;
            right = false;
        }

        public bool AnyCollision()
        {
            return (above || below || left || right);
        }
    }

    private int facingDirection;
    public int LastDirection { get => facingDirection; set => facingDirection = value; }

    [SerializeField] LayerMask collisionMask;
    public LayerMask CollisionMask { get => collisionMask; set => collisionMask = value; }

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    [SerializeField] private int horizontalRayCount = 4;
    [SerializeField] private int verticalRayCount = 4;
    float horizontalRaySpacing;
    float verticalRaySpacing;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        facingDirection = 1;

        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        collisions.Reset();

        UpdateRaycastOrigins();

        if (velocity.x != 0)
        {
            facingDirection = (int)Mathf.Sign(velocity.x);
        }

        HorizontalCollisions(ref velocity);

        if (velocity.y != 0)
            VerticalCollisions(ref velocity);

        transform.Translate(velocity);
    }

    public void MoveToPoint(Vector3 point, float speed)
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
        Vector3 direction = (newPos - point).normalized;

        collisions.Reset();

        UpdateRaycastOrigins();

        if (direction.x != 0)
        {
            facingDirection = (int)Mathf.Sign(direction.x);
        }

        HorizontalCollisions(ref direction);

        if (direction.y != 0)
            VerticalCollisions(ref direction);

        transform.position = newPos;
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLenght = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLenght, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLenght = hit.distance;

                collisions.above = directionY == 1;
                collisions.below = directionY == -1;
            }
        }
    }

    /// <summary>
    /// Checks the horizontal collisions and adjust the velocity in consequence
    /// </summary>
    /// <param name="velocity"></param>
    public void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = facingDirection;
        float rayLenght = Mathf.Abs(velocity.x) + skinWidth;

        //This helps to detect the walls easily.
        if (Mathf.Abs(velocity.x) < skinWidth)
            rayLenght = 2.0f * skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLenght, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;

                rayLenght = hit.distance;

                if (Mathf.Abs(velocity.x) < skinWidth)
                    rayLenght = 2.0f * skinWidth;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;

                collisions.horzCollsNumber++;
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
}
