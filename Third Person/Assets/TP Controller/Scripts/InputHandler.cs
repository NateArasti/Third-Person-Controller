using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    public static Vector3 PlayerMovementAxis { get; private set; }
    public static bool IsRunning { get; private set; }

    public static readonly UnityEvent TryRoll = new UnityEvent();

    private static InputHandler _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("InputHandler already exists!");
            Destroy(this);
        }
        _instance = this;
    }

    private void Update()
    {
        PlayerMovementAxis = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        if(Input.GetKeyDown(KeyCode.Space)) TryRoll.Invoke();
        IsRunning = Input.GetKey(KeyCode.LeftShift);
    }
}
