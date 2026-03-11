using System;
using Unity.Netcode;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;


public class VRConnectionManager : MonoBehaviour
{
    [Header("Configuration VR & Réseau")]
    public string profileName = "JoueurVR";
    public string sessionName = "test";
    public string serverIPAddress = "192.168.72.86";

    private int _maxPlayers = 10;
    private ConnectionState _state = ConnectionState.Disconnected;
    private ISession _session;
    private NetworkManager m_NetworkManager;

    private enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected,
    }

    private async void Awake()
    {
        m_NetworkManager = GetComponent<NetworkManager>();
        m_NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        m_NetworkManager.OnSessionOwnerPromoted += OnSessionOwnerPromoted;
        await UnityServices.InitializeAsync();
    }

    private void OnSessionOwnerPromoted(ulong sessionOwnerPromoted)
    {
        if (m_NetworkManager.LocalClient.IsSessionOwner)
        {
            Debug.Log($"Client-{m_NetworkManager.LocalClientId} is the session owner!");
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        if (m_NetworkManager.LocalClientId == clientId)
        {
            Debug.Log($"Client-{clientId} is connected and can spawn {nameof(NetworkObject)}s.");
        }
    }

    public void JoinSharedWorld()
    {
        if (_state == ConnectionState.Connected || _state == ConnectionState.Connecting)
            return;

        Debug.Log($"[VR] Rejoindre le monde - Joueur: {profileName}, Session: {sessionName}");
        CreateOrJoinSessionAsyncDirect();
    }

    private async Task CreateOrJoinSessionAsyncDirect()
    {
        _state = ConnectionState.Connecting;

        var transport = m_NetworkManager.GetComponent<UnityTransport>();
        if (transport != null)
        {
            transport.ConnectionData.Address = serverIPAddress;
        }

        try
        {
            AuthenticationService.Instance.SwitchProfile(profileName);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            var options = new SessionOptions()
            {
                Name = sessionName,
                MaxPlayers = _maxPlayers
            }.WithDistributedAuthorityNetwork();

            _session = await MultiplayerService.Instance.CreateOrJoinSessionAsync(sessionName, options);

            _state = ConnectionState.Connected;
            Debug.Log("[VR] Connexion réussie !");
        }
        catch (Exception e)
        {
            _state = ConnectionState.Disconnected;
            Debug.LogException(e);
        }
    }

    private void OnDestroy()
    {
        _session?.LeaveAsync();
    }
}