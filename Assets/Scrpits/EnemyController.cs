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
    [SerializeField] private GameObject idlePlace;

    private Transform target;
    private Vector3 moveDirection;

    [SerializeField] float maxDistance = 2.0f;
    [SerializeField] float minDistance = 5.0f;
    private float distance;

    [SerializeField] private LayerMask isGround;
    private EnemyType enemyType = EnemyType.Follow;

    private void OnTriggerEnter(Collider other)
    {
        //.Instance.UpdateGameState(GameState.GameOver);
        
    }

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
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        distance = Vector3.Distance(target.position, transform.position);
        if (enemyType == EnemyType.Follow)
        {
          //  if (distance > maxDistance)
          //  {
                characterController.Move(moveDirection * speed * Time.deltaTime);
          //  }
        }
        if (enemyType == EnemyType.RunAway)
        {
            if (distance < minDistance)
            {
                characterController.Move(moveDirection * speed * Time.deltaTime);
            }
        }
        if (enemyType == EnemyType.IdleChase)
        {
           // if (distance < minDistance)
            //{
                characterController.Move(moveDirection * speed * Time.deltaTime);
            //}
            //else
           // {
               // characterController.Move(moveDirection * speed * Time.deltaTime);
            //}
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
    {/*
        if (target)
        {
            direction = (target.position - transform.position);
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            moveDirection = direction;
        }*/
        if (target)
        {
            if (enemyType == EnemyType.Follow)
            {
                direction = (target.position - transform.position);
            }
            if (enemyType == EnemyType.RunAway)
            {
                direction = (transform.position - target.position);
            }

            if (enemyType == EnemyType.IdleChase)
            {
                if (distance < minDistance)
                {
                    direction = (target.position - transform.position);
                }
                else
                {
                    direction = (idlePlace.transform.position - transform.position);
                }
                
            }
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            moveDirection = direction;
        }
    }

    private void OnEnable()
    {
        GameManager.OnStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameState state)
    {
        if (state == GameState.Follow)
        {
            enemyType = EnemyType.Follow;
        }
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= OnGameStateChange;
    }

}

public enum EnemyType
{
    Follow,
    RunAway,
    IdleChase
}