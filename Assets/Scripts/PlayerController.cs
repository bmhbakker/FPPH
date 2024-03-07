using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private Rigidbody _rigidbody;

    [SerializeField] 
    private GameObject _cameraHolder;
    
    [SerializeField] 
    private float _moveSpeed;
    
    [SerializeField] 
    private float _lookSensitivity;
    
    [SerializeField] 
    private float _maxForce;
    
    private Vector2 _playerMove;
    
    private Vector2 _playerLook;

    private float _lookRotation;
 
    private void Start()
    {
        Debug.Log("Start");
    }
    
    private void OnMove(InputValue inputValue)
    {
        _playerMove = inputValue.Get<Vector2>();
    }
    
    private void OnLook(InputValue inputValue)
    {
        _playerLook = inputValue.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        transform.Rotate(Vector3.up* _playerLook.x * _lookSensitivity);

        _lookRotation += (-_playerLook.y * _lookSensitivity);
        _cameraHolder.transform.eulerAngles = new Vector3(_lookRotation, _cameraHolder.transform.eulerAngles.y,
            _cameraHolder.transform.eulerAngles.z);
    }

    private void Move()
    {
        var currentVelocity = _rigidbody.velocity;
        var targetVelocity = new Vector3(_playerMove.x, 0, _playerMove.y);
        targetVelocity *= _moveSpeed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        var velocityChange = (targetVelocity - currentVelocity);

        Vector3.ClampMagnitude(velocityChange, _maxForce);
        
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }
}
