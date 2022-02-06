using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private float _normalMoveSpeed = 5;
    [SerializeField] private float _speedUpValue = 2;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    private CharacterController _characterController;
    private Transform _transform;
    private float _smoothRotateVelocity;
    private Vector3 _movement;
    private bool _inRoll;

    public bool IsMoving => _movement != Vector3.zero;

    public readonly UnityEvent Roll = new UnityEvent();

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _transform = transform;
        InputHandler.TryRoll.AddListener(() =>
        {
            if (!IsMoving || _inRoll) return;
            Roll.Invoke();
            _inRoll = true;
            StartCoroutine(UnityExtensions.InvokeAfterDelayCoroutine(() => _inRoll = false, 1.05f));
        });
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if(_inRoll) return;
        _movement = InputHandler.PlayerMovementAxis.normalized;

        var moveSpeed = _normalMoveSpeed;
        if (InputHandler.IsRunning) moveSpeed *= _speedUpValue;

        if (_movement.magnitude <= 0.1f) return;

        var targetAngle = Mathf.Atan2(_movement.x, _movement.z) * Mathf.Rad2Deg +
                          UnityExtensions.MainCamera.transform.eulerAngles.y;
        var angle = Mathf.SmoothDampAngle(
            _transform.eulerAngles.y,
            targetAngle,
            ref _smoothRotateVelocity,
            _turnSmoothTime);

        _transform.rotation = Quaternion.Euler(0, angle, 0);

        _movement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        _characterController.Move(_movement.normalized * (moveSpeed * Time.deltaTime));
    }

    //private bool IsGrounded()
    //{
    //    return Physics.Raycast(
    //        _transform.position, 
    //        Vector3.down, 
    //        0.05f);
    //}
}
