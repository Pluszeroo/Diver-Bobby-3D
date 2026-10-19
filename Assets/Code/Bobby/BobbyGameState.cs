using UnityEngine;

public class BobbyGameState : MonoBehaviour
{
    public static BobbyGameState Instance;

    [field:SerializeField] public BobbyState CurrentState { get; private set; } = BobbyState.Idle;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBobbyState(BobbyState newState)
    {
        CurrentState = newState;
    }
}

public enum BobbyState
{
    Idle,
    Dialogue,
    Map,
    Stele,
    CutScene,
    Popup,
}
