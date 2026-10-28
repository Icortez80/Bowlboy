using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAimAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference dashAction;
    [SerializeField] private InputActionReference aimLockAction;
    [SerializeField] private InputActionReference specialAction;
    [SerializeField] private float pressThresh = 0.5f;   // must pass this to register
    [SerializeField] private float releaseThresh = 0.3f; // must fall below this to clear

    private Vector2 digitalInput;
    public Vector2 MoveAim => digitalInput;
    public bool JumpPressed => jumpAction.action.WasPressedThisFrame();
    public bool JumpHeld => jumpAction.action.IsPressed();
    public bool DashPressed => dashAction.action.WasPressedThisFrame();
    public bool ShootHeld => shootAction.action.IsPressed();
    public bool AimLocked => aimLockAction.action.IsPressed();
    public bool SpecialPressed => specialAction.action.WasPressedThisFrame();

    void OnEnable()
    {
        moveAimAction.action.Enable();
        jumpAction.action.Enable();
        shootAction.action.Enable();
        dashAction.action.Enable();
        aimLockAction.action.Enable();
        specialAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAimAction.action.Disable();
        jumpAction.action.Disable();
        shootAction.action.Disable();
        dashAction.action.Disable();
        aimLockAction.action.Disable();
        specialAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 rawInput = moveAimAction.action.ReadValue<Vector2>();

        digitalInput.x = ApplyHysteresis(rawInput.x, (int)digitalInput.x);
        digitalInput.y = ApplyHysteresis(rawInput.y, (int)digitalInput.y);
    }

    //this function is to help debounce the stick on controller
    private int ApplyHysteresis(float raw, int current)
    {
        // Strong push registers immediately, including a direct flip from one side to the other
        if (raw > pressThresh) return 1;
        if (raw < -pressThresh) return -1;

        // In the "gray zone" between release and press, hold the previous state
        if (current == 1 && raw > releaseThresh) return 1;
        if (current == -1 && raw < -releaseThresh) return -1;

        return 0;
    }
}
