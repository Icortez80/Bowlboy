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
    [SerializeField] private float axisThresh = 0.4f;
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
        //X axis check
        if(rawInput.x > axisThresh)
        {
            digitalInput.x = 1;

        }else if( rawInput.x < -axisThresh)
        {
            digitalInput.x = -1;
        }
        else
        {
            digitalInput.x = 0;
        }

        //Y axis check
        if (rawInput.y > axisThresh)
        {
            digitalInput.y = 1;

        }
        else if (rawInput.y < -axisThresh)
        {
            digitalInput.y = -1;
        }
        else
        {
            digitalInput.y = 0;
        }

        //ebug.Log(digitalInput);
    }
}
