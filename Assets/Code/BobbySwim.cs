using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class BobbySwim : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float maxSpeed = 5f;
    //[SerializeField] private float acceleration = 15f;
    [SerializeField] private float damping = 0.88f;
    [SerializeField] private float Buoyancy = 3f;

    [SerializeField] private CameraMangager manager;

    private Rigidbody2D rb;
    private BobbyControls controls;
    private Vector2 moveInput;

    [Header("Bobbing")]
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobAmount = 1.5f;
    private float bobTimer = 0f;

    //private float lastUpPressTime = -1f;
    //private float doubleTapThreshold = 0.3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
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
    } 

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.DialogueInteract.performed -= OnSpacePerformed;
        controls.Player.Interact.performed -= OnInteractPerformed;
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

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        QuestManager.GetInstance().TryInteract();

        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            DialogueManager.GetInstance().OnSpacePressed(ctx);
            return;
        }

        if (DialogueManager.GetInstance().closestTrigger != null)
        {
            DialogueManager.GetInstance().EnterDialogueMode();
            return;
        }

        if (manager.isinPuzzleMode)
        {
            manager.ExitPuzzleMode();
            return;
        }
        //if (manager.isinPuzzleMode)
        //{
        //    manager.ExitPuzzleMode();
        //}
    }

    private void FixedUpdate()
    {
        
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        
        if (moveInput != Vector2.zero)
        {
            manager.SwitchToFollowBobbyCamera();

            rb.AddForce(moveInput.normalized * moveForce);

            //Restrict the maxspeed, don't make it as unlimited acceleration please
            if(rb.linearVelocity.magnitude > maxSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        else
        {
            //rb.linearVelocity *= damping;

            //if (rb.linearVelocity.magnitude < 0.01f)
            //    rb.linearVelocity = Vector2.zero;

            //bobTimer += Time.fixedDeltaTime;
            //float bob = Mathf.Sin(bobTimer * bobSpeed) * bobAmount;
            //rb.AddForce(new Vector2(0f, bob));

            rb.linearVelocity = new Vector2(rb.linearVelocity.x * damping, rb.linearVelocity.y);

            if (Mathf.Abs(rb.linearVelocity.x) < 0.01f)
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            bobTimer += Time.fixedDeltaTime;
            float bobY = Mathf.Sin(bobTimer * bobSpeed) * bobAmount;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bobY);



        }

        //{
        //    // When player press the button
        //    // Bobby! How fast you can
        //    //Vector2 targetVelocity = moveInput.normalized * swimSpeed;
        //    //rb.linearVelocity = Vector2.MoveTowards(
        //    //    rb.linearVelocity,
        //    //    targetVelocity,
        //    //    acceleration * Time.fixedDeltaTime
        //    );

        //    // TODO: need to refactor linearVelocity assignment,
        //    // instead use rb.AddForce so that we can add move velocity + bubble velocity (up direction)
        //}
        //else
        //{
        //    // When player release the button
        //    rb.linearVelocity *= damping;

        //    if (rb.linearVelocity.magnitude < 0.01f)
        //        rb.linearVelocity = Vector2.zero;
        //}
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
