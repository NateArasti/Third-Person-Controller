using UnityEngine;

[RequireComponent(typeof(Animator), typeof(ThirdPersonMovement))]
public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private int _randomIDLECount = 3;
    [SerializeField] private float _idleChangeDelta = 2f;
    [SerializeField] private float _calmActivateTime = 5f;
    private float _lastRandomIDLEUsed;
    private float _timeStayingCalm;

    private Animator _animator;
    private ThirdPersonMovement _thirdPersonMovement;

    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Roll = Animator.StringToHash("Roll");
    private static readonly int RunStop = Animator.StringToHash("RunStop");
    private static readonly int IDLE = Animator.StringToHash("IDLE");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _thirdPersonMovement = GetComponent<ThirdPersonMovement>();
        _thirdPersonMovement.Roll.AddListener(() => _animator.SetTrigger(Roll));
    }

    private void Update()
    {
        var axis = InputHandler.PlayerMovementAxis;
        var isMoving = _thirdPersonMovement.IsMoving;
        _animator.SetFloat(Horizontal, axis.x);
        _animator.SetFloat(Vertical, axis.z);
        _animator.SetBool(Walking, axis != Vector3.zero);
        _animator.SetBool(Running, InputHandler.IsRunning && isMoving);
        _animator.SetBool(RunStop, InputHandler.IsRunning && !isMoving);
        HandleIDLEAnimations(isMoving);
    }

    private void HandleIDLEAnimations(bool isMoving)
    {
        if (!isMoving)
        {
            _timeStayingCalm += Time.deltaTime;
            if (_timeStayingCalm >= _calmActivateTime)
            {
                _animator.SetInteger(IDLE, 0);
            }
            else if (Time.time - _lastRandomIDLEUsed >= _idleChangeDelta)
            {
                _animator.SetInteger(IDLE, Random.Range(1, _randomIDLECount + 1));
                _lastRandomIDLEUsed = Time.time;
                StartCoroutine(UnityExtensions.InvokeAfterDelayCoroutine(
                    () => _animator.SetInteger(IDLE, -1), 1f)
                );
            }
        }
        else
        {
            _timeStayingCalm = 0;
            _lastRandomIDLEUsed = Time.time;
            _animator.SetInteger(IDLE, -1);
        }
    }
}
