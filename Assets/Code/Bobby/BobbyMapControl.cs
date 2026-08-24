using UnityEditor.ShaderGraph;
using UnityEngine;

public class BobbyMapControl : MonoBehaviour
{
    [SerializeField] private GameObject _map;

    private BobbyControls _controls;

    private void Awake()
    {
        _controls = new BobbyControls();
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _controls.Player.Map.performed += Map_performed;
    }

    private void OnDisable()
    {
        _controls.Player.Map.performed -= Map_performed;
        _controls.Player.Disable();
    }

    private void Map_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!CanOpenMap()) return;

        if (_map.activeSelf == true)
        {
            BobbyGameState.Instance.SetBobbyState(BobbyState.Idle);
            _map.SetActive(false);
        }
        else
        {
            BobbyGameState.Instance.SetBobbyState(BobbyState.Map);
            _map.SetActive(true);
        }
        
    }

    public bool CanOpenMap()
    {
        return BobbyGameState.Instance.CurrentState switch
        {
            BobbyState.Idle => true,
            BobbyState.Dialogue => false,
            BobbyState.Map => true,
            _ => throw new System.NotImplementedException(),
        };
    }
}
