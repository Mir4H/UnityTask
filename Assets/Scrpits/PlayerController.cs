using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;

    [SerializeField] private float speed = 5f;

    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiply = 3.0f;
    private float _velocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Update()
    {
        ApplyRotation();
        ApplyMovement();
        ApplyGravity();
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;
        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
    private void ApplyMovement()
    {
        _characterController.Move(_direction * speed * Time.deltaTime);
    }
    private void ApplyGravity()
    {
        _velocity += _gravity * gravityMultiply * Time.deltaTime;
        _direction.y = _velocity;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }
}
