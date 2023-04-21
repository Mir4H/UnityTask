using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 input;
    private CharacterController characterController;
    private Vector3 direction;

    [SerializeField] private float smoothTime = 0.05f;
    private float currentVelocity;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;

    private float gravity = -10f;
    [SerializeField] private float gravityMultiply = 3.0f;
    private float velocity;

    [SerializeField] private GameObject enemyStartPosition;
    [SerializeField] private GameObject playerStartPosition;
    private bool GameOver = true;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyController>())
        {
            GameOver = true;
            other.transform.position = enemyStartPosition.transform.position;
            GameManager.Instance.UpdateGameState(GameState.GameOver);
        }
    }

    private void OnEnable()
    {
        GameManager.OnStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameState state)
    {
        if (state == GameState.Follow || state == GameState.IdleChase || state == GameState.RunAway)
        {
            GameOver = false;
        }
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= OnGameStateChange;
    }

    public void Update()
    {
        if (!GameOver)
        {
            ApplyRotation();
            ApplyMovement();
            ApplyGravity();
        }
        else
        {
            gameObject.transform.position = playerStartPosition.transform.position;
        }
    }

    private void ApplyRotation()
    {
        if (input.sqrMagnitude == 0) return;
        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
    private void ApplyMovement()
    {
        float speed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
        characterController.Move(direction * speed * Time.deltaTime);
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

        direction.y = velocity;
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0.0f, input.y);
    }
}
