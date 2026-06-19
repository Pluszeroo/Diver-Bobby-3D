using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BobbySwim : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float maxSpeed = 5f;
    //[SerializeField] private float acceleration = 15f;
    [SerializeField] private float damping = 0.88f;
    [SerializeField] private float Buoyancy = 3f;
    [SerializeField] private float sprintMultiplier = 2f;

    [SerializeField] private CameraMangager manager;

    private Rigidbody rb;
    private BobbyControls controls;
    private Vector2 moveInput;
    private bool isSprinting = false;

    [Header("Bobbing")]
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobAmount = 1.5f;
    private float bobTimer = 0f;

    //private float lastUpPressTime = -1f;
    //private float doubleTapThreshold = 0.3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        //rb.linearDamping = 0f;

        //initilize controls
        controls = new BobbyControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.DialogueInteract.performed += OnSpacePerformed;
        controls.Player.Interact.performed += OnInteractPerformed;
        controls.Player.Sprint.performed += OnSprintPerformed;
        controls.Player.Sprint.canceled += OnSprintCanceled;
        controls.Player.Skip.started += OnSkipStarted;
        controls.Player.Skip.canceled += OnSkipCanceled;
    } 

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.DialogueInteract.performed -= OnSpacePerformed;
        controls.Player.Interact.performed -= OnInteractPerformed;
        controls.Player.Sprint.performed -= OnSprintPerformed;
        controls.Player.Sprint.canceled -= OnSprintCanceled;
        controls.Player.Skip.started -= OnSkipStarted;
        controls.Player.Skip.canceled -= OnSkipCanceled;
        controls.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    private void OnSpacePerformed(InputAction.CallbackContext ctx)
    {
        DialogueManager.GetInstance().OnSpacePressed(ctx);
    }

    private void OnSprintPerformed(InputAction.CallbackContext ctx)
    {
        isSprinting = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext ctx)
    {
        isSprinting = false;
    }

    private void OnSkipStarted(InputAction.CallbackContext ctx)
    {
        DialogueManager.GetInstance().OnSkipHoldStarted();
    }

    private void OnSkipCanceled(InputAction.CallbackContext ctx)
    {
        DialogueManager.GetInstance().OnSkipHoldCanceled();
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {

        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            DialogueManager.GetInstance().OnSpacePressed(ctx);
            return;
        }

        //if (InventoryManager.Instance.IsOpen) return;

        if (QuestManager.GetInstance().TryInteract()) return;

        if (DialogueManager.GetInstance().closestTrigger != null)
            DialogueManager.GetInstance().EnterDialogueMode();
    }

    private void FixedUpdate()
    {
        
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }
        
        if (moveInput != Vector2.zero)
        {
            manager.SwitchToFollowBobbyCamera();

            float currentForce = isSprinting ? moveForce * sprintMultiplier : moveForce;
            rb.AddForce(new Vector3(moveInput.x, moveInput.y, 0) * currentForce);

            float currentMaxSpeed = isSprinting ? maxSpeed * sprintMultiplier : maxSpeed;
            if (rb.linearVelocity.magnitude > currentMaxSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * currentMaxSpeed;
            //Restrict the maxspeed, don't make it as unlimited acceleration please
            //if (rb.linearVelocity.magnitude > maxSpeed)
            //    rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        else
        {
            //rb.linearVelocity *= damping;

            //if (rb.linearVelocity.magnitude < 0.01f)
            //    rb.linearVelocity = Vector2.zero;

            //bobTimer += Time.fixedDeltaTime;
            //float bob = Mathf.Sin(bobTimer * bobSpeed) * bobAmount;
            //rb.AddForce(new Vector2(0f, bob));

            bobTimer += Time.fixedDeltaTime;
            float bobY = Mathf.Sin(bobTimer * bobSpeed) * bobAmount;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x * damping, bobY, 0f);

            if (Mathf.Abs(rb.linearVelocity.x) < 0.01f)
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y);

            //bobTimer += Time.fixedDeltaTime;
            //float bobY = Mathf.Sin(bobTimer * bobSpeed) * bobAmount;
            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, bobY);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
