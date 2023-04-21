using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float smoothTime = 0.05f;
    private float velocity;
    [SerializeField] private float gravityMultiply = 3.0f;
    private Vector3 direction = Vector3.zero;
    private float gravity = -10f;

    private float currentVelocity;

    private CharacterController characterController;
    private PlayerController player;
    [SerializeField] private GameObject EnemySpotOne;
    [SerializeField] private GameObject EnemySpotTwo;

    private Transform target;
    private Vector3 moveDirection;

    [SerializeField] float maxDistance = 2.0f;
    [SerializeField] float minDistance = 5.0f;
    private float distance;

    [SerializeField] private LayerMask isGround;
    private GameState gameState;
    private bool gamePaused = true;

    public void Awake()
    {
        characterController = GetComponent<CharacterController>();
        player = FindObjectOfType<PlayerController>();
    }

    public void Start()
    {
        target = player.transform;
    }

    private void Update()
    {
        if (!gamePaused)
        {
            ApplyRotation();
            ApplyGravity();
            ApplyMovement();
        }
    }

    private void ApplyMovement()
    {
        
            distance = Vector3.Distance(target.position, transform.position);
        if (gameState == GameState.Follow)
        {
             
             characterController.Move(moveDirection * speed * Time.deltaTime);
        }
        if (gameState == GameState.RunAway)
        {
            
            if (distance < minDistance)
            {
                characterController.Move(moveDirection * speed * Time.deltaTime);
            }
        }
            if (gameState == GameState.IdleChase)
            {
            

            characterController.Move(moveDirection * speed * Time.deltaTime);

        }
        
        



    }

    private void ApplyGravity()
    {
        
        if (characterController.isGrounded && velocity < 0.0f)
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += gravity * gravityMultiply * Time.deltaTime;
        }

        moveDirection.y = velocity;
    }

    private void ApplyRotation()
    {
        if (target)
        {
            if (gameState == GameState.Follow)
            {
                direction = (target.position - transform.position);
            }
            if (gameState == GameState.RunAway)
            {
                direction = (transform.position - target.position);
            }

            if (gameState == GameState.IdleChase)
            {
               // direction = Vector3.Lerp((EnemySpotOne.transform.position - EnemySpotTwo.transform.position), (EnemySpotTwo.transform.position - EnemySpotOne.transform.position), Mathf.PingPong(Time.time * speed, 1.0f));

                if (distance < minDistance)
                {
                    direction = (target.position - transform.position);
                }
                else
                {
                    //direction = (EnemySpotOne.transform.position - transform.position);
                    direction = Vector3.Lerp((EnemySpotOne.transform.position - transform.position), (EnemySpotTwo.transform.position - EnemySpotOne.transform.position), Mathf.PingPong(Time.time * speed, 1.0f));

                }

            }
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            moveDirection = direction;
        }
    }

    private void OnEnable()
    {
        GameManager.OnStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameState state)
    {
        gameState = state;
        if (gameState == GameState.IdleChase || gameState == GameState.Follow || gameState == GameState.RunAway)
        {
            gamePaused = false;
        }
        else
        {
            gamePaused = true;
        }
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= OnGameStateChange;
    }

}
/*
public enum EnemyType
{
    Follow,
    RunAway,
    IdleChase
}*/