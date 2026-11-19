using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    [SerializeField] CharacterController _charController;
    private Vector3 _direction;
    [SerializeField] Transform _camera;

    [SerializeField] float _rotationSpeed = 500f;
    private Camera _mainCamera;

    [SerializeField] float _speed;

    public float Speed { get; set; }

    public void ResetStats() => Speed = _speed;

    private float _gravity = -9.81f;
    [SerializeField] float _gravityMultiplier;
    private float _velocity;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        ResetStats();
    }

    private void Update()
    {

        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0f)
        {
            _velocity = -1f;
        }
        else
        {
            _velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        _direction = Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * new Vector3(_input.x, 0f, _input.y);
        var targetRotation = Quaternion.LookRotation(_direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        _charController.Move(_direction * Speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();

        _direction = new Vector3(_input.x, 0f, _input.y);
    }

    private bool IsGrounded() => _charController.isGrounded;
}
