using UnityEngine;

public class XYHandler : MonoBehaviour
{
    [SerializeField] private TcpJsonXYClient receiver;

    private void OnEnable()
    {
        receiver.PositionReceived += HandlePosition;
    }

    private void OnDisable()
    {
        receiver.PositionReceived -= HandlePosition;
    }

    private void HandlePosition(XYMessage message)
    {
        Debug.Log($"X: {message.x}, Y: {message.y}");
    }
}
