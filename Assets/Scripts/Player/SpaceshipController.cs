using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceship : OutOfScreenTeleporter
{
    private readonly Transform transform;
    private readonly List<Weapon> weapons;
    private readonly IGraphics moveGraphics;
    private readonly IPlayerInput input;
    private readonly float acceleration;
    private readonly float deceleration;
    private readonly float rotateSpeed;

    private const float velocityThreshold = 0.0001f;

    private Weapon currentWeapon;
    private Vector2 velocity;
    private int currentRotateDirection;
    private bool isMoving;
    private bool isRotating;
    
    public event Action<float> RotationChanged;
    public event Action<Vector2> PositionChanged;
    public event Action<float> SpeedChanged;
    public event Action Destroyed;

    protected override Vector2 CurrentPosition { get => transform.position; set => transform.position = value; }

    public PlayerSpaceship(Transform transform, IGraphics moveGraphics, IPlayerInput input, ICollisionEventSender collisionEventSender, List<Weapon> weapons, IHitSender hitSender,
        float acceleration, float deceleration, float rotateSpeed, Vector2 sceneDimension) : base(sceneDimension)
    {
        this.input = input;
        this.input.OnSwitchWeapon += SetActiveWeapon;
        this.input.OnFire += Input_OnFire;
        this.input.OnStartMoving += Input_OnStartMoving;
        this.input.OnStopMoving += Input_OnStopMoving;
        this.input.OnStartTurning += Input_OnStartTurning;
        this.input.OnStopTurning += Input_OnStopTurning;

        hitSender.OnHit += OnHit;

        collisionEventSender.OnCollisionEnter += OnCollisionEnter;

        this.moveGraphics = moveGraphics;
        this.transform = transform;
        this.weapons = weapons;
        this.acceleration = acceleration;
        this.deceleration = deceleration;
        this.rotateSpeed = rotateSpeed;
    }

    private void OnHit()
    {
        Destroy();
    }

    private void OnCollisionEnter()
    {
        Destroy();
    }

    private void Destroy()
    {
        transform.gameObject.SetActive(false);

        input.OnFire -= Input_OnFire;
        input.OnStartMoving -= Input_OnStartMoving;
        input.OnStopMoving -= Input_OnStopMoving;
        input.OnStartTurning -= Input_OnStartTurning;
        input.OnStopTurning -= Input_OnStopTurning;

        Destroyed?.Invoke();
    }

    public void SetActiveWeapon(int index)
    {
        currentWeapon = weapons[index];
    }

    private void Input_OnStopTurning()
    {
        isRotating = false;
    }

    private void Input_OnStartTurning(Vector2 obj)
    {
        isRotating = true;
        currentRotateDirection = -(int)obj.x;
    }

    private void Input_OnStopMoving()
    {
        isMoving = false;
        moveGraphics.SetActive(false);
    }

    private void Input_OnStartMoving()
    {
        isMoving = true;
        moveGraphics.SetActive(true);
    }

    private void Input_OnFire()
    {
        if (currentWeapon.CanShootNow)
        {
            currentWeapon.Shoot();
        }
    }

    public override void Update(float deltaTime)
    {
        if (isMoving)
        {
            velocity += acceleration * deltaTime * (Vector2)transform.right;
        }
        else
        {
            if (velocity.magnitude > velocityThreshold)
            {
                velocity -= deceleration * deltaTime * velocity.normalized;
            }
            else
            {
                velocity = Vector2.zero;
            }
        }

        SpeedChanged?.Invoke(velocity.magnitude);

        transform.Translate(velocity * Time.deltaTime, Space.World);
        PositionChanged?.Invoke(transform.position);

        if (isRotating)
        {
            transform.Rotate(new Vector3(0f, 0f, currentRotateDirection * deltaTime * rotateSpeed));
            RotationChanged?.Invoke(transform.eulerAngles.z);
        }

        base.Update(deltaTime);
    }
}
