using UnityEngine;

public class BobbyMapControl : MonoBehaviour
{
    [SerializeField] private GameObject _map;



    public bool CanOpenMap()
    {
        return BobbyGameState.Instance.CurrentState switch
        {
            BobbyState.Idle => true,
            BobbyState.Dialogue => false,
            BobbyState.Map => false,
            _ => throw new System.NotImplementedException(),
        };
    }
}
