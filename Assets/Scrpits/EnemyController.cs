using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 0.8f;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float gravityMultiply = 3.0f;
    [SerializeField] private GameObject EnemySpotOne;
    [SerializeField] private GameObject EnemySpotTwo;
    [SerializeField] private GameObject OffSetSpot;
    [SerializeField] float maxDistance = 4.0f;
    [SerializeField] LayerMask layerMask;

    private float velocity;
    private Vector3 direction = Vector3.zero;
    private float gravity = -10f;
    private float currentVelocity;
    private CharacterController characterController;
    private PlayerController player;
    private Transform target;
    private Vector3 moveDirection;
    private float distance;
    private GameState gameState;
    private bool gamePaused = true;
    bool isDirSafe = true;
    private float angle;
    private bool moveI;

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
            ApplyGravity();
            ApplyMovement();
        }
    }

    private void FixedUpdate()
    {
        ApplyRotation();
        RayCastWalls();
        distance = Vector3.Distance(target.position, transform.position);
        if (distance < 5) moveI = true;
        if (distance > 6) moveI = false;
    }

    private void RayCastWalls()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.collider.tag == "wall")
            {
                 isDirSafe = false;
            }
            if (hit.collider.tag == "center")
            {
                 isDirSafe = true;
            }
        }
    }

    private void ApplyMovement()
    {
        if (gameState == GameState.Follow || gameState == GameState.IdleChase)
        {
             characterController.Move(moveDirection * speed * Time.deltaTime);
        }
        if (gameState == GameState.RunAway)
        {
            speed = 3f;
            if (moveI)
            { 
                characterController.Move(moveDirection * speed * Time.deltaTime);
            }
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
                
                if (isDirSafe)
                {
                    direction = (transform.position - target.position).normalized;
                }
                else
                {
                    direction = (transform.position - OffSetSpot.transform.position).normalized;
                }
            }

            if (gameState == GameState.IdleChase)
            {
                if (distance < maxDistance)
                {
                    direction = (target.position - transform.position);
                }
                else
                {
                    direction = Vector3.Lerp((EnemySpotOne.transform.position - transform.position), (EnemySpotTwo.transform.position - EnemySpotOne.transform.position), Mathf.PingPong(Time.time * speed, 1.0f));
                }
            }
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
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
            speed = 0.8f;
            gamePaused = true;
        }
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= OnGameStateChange;
    }
}