using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent enemyAgent;
    
    [SerializeField] private Transform target;
    [SerializeField] private GameObject EnemySpotOne;
    [SerializeField] private GameObject EnemySpotTwo;
    [SerializeField] float playerDistance = 5.0f;
    [SerializeField] private float smoothTime = 0.05f;
    private Vector3 direction = Vector3.zero;
    private bool gamePaused = true;
    private float speed;
    private float distance;
    private List<Transform> EnemySpots;
    private int destinationSpot = 0;
    private float currentVelocity;
    private GameState gameState;

    public void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        EnemySpots = new List<Transform>() { EnemySpotOne.transform, EnemySpotTwo.transform };
        speed = enemyAgent.speed;
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, target.position);
        if (!gamePaused)
        {
            ApplyRotation();
            ApplyMovement();
        }
        else
        {
            enemyAgent.speed = 10f;
            enemyAgent.SetDestination(EnemySpotOne.transform.position);
        }
    }

    private void ApplyMovement()
    {
        
        if (gameState == GameState.Follow)
        {
            enemyAgent.SetDestination(target.position);
        }
        if (gameState == GameState.RunAway)
        {
            
            if (distance < playerDistance)
            {
                Vector3 playerDir = transform.position - target.position;
                Vector3 newPos = transform.position + playerDir;
                enemyAgent.SetDestination(newPos);
            }
        }
        if (gameState == GameState.IdleChase)
        {

            if (distance > playerDistance)
            {
                if (EnemySpots.Count == 0) return;
                enemyAgent.autoBraking = false;
                if (!enemyAgent.pathPending && enemyAgent.remainingDistance < 1f)
                {
                    enemyAgent.SetDestination(EnemySpots[destinationSpot].position);
                    destinationSpot = (destinationSpot + 1) % EnemySpots.Count;
                }
            }
            else
            {
                enemyAgent.SetDestination(target.position);
            } 
        }
    }

    private void ApplyRotation()
    {
        if (target)
        {
            if (gameState == GameState.Follow) direction = (target.position - transform.position);
            if (gameState == GameState.RunAway) direction = (transform.position - target.position);
            if (gameState == GameState.IdleChase)
            {
                if (distance < playerDistance)
                {
                    smoothTime = 0.05f;
                    direction = (target.position - transform.position);
                }
                else
                {
                    smoothTime = 0.2f;
                    if (!enemyAgent.pathPending && enemyAgent.remainingDistance < 1f)
                    {
                        direction = EnemySpots[destinationSpot].position - transform.position;
                    }
                }
            }
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
    private void OnEnable()
    {
        GameManager.OnStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameState state)
    {
        gameState = state;
        if (gameState == GameState.IdleChase || gameState == GameState.Follow)
        {
            enemyAgent.speed = speed;
            smoothTime = 0.05f;
            gamePaused = false;
        }
        else if (gameState == GameState.RunAway)
        {
            enemyAgent.speed = 3.5f;
            smoothTime = 0.05f;
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
