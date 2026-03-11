using Unity.Netcode;
using UnityEngine;

public class PlayerColorizer : NetworkBehaviour
{

    public MeshRenderer playerMeshRenderer;

    private NetworkVariable<Color> playerColor = new NetworkVariable<Color>(
        Color.white,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        playerColor.OnValueChanged += OnColorChanged;

        if (IsServer)
        {
            playerColor.Value = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
        }
        else
        {
            ApplyColor(playerColor.Value);
        }
    }

    public override void OnNetworkDespawn()
    {
        playerColor.OnValueChanged -= OnColorChanged;
    }

    private void OnColorChanged(Color previousValue, Color newValue)
    {
        ApplyColor(newValue);
    }

    private void ApplyColor(Color newColor)
    {
        if (playerMeshRenderer != null)
        {
            playerMeshRenderer.material.color = newColor;
        }
    }
}