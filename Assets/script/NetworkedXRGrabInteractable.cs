using UnityEngine;

using Unity.Netcode;


public class NetworkedXRGrabInteractable : NetworkBehaviour
{

    protected NetworkObject networkObject;

    private Color catchableColor = Color.cyan;

    private Color caughtColor = Color.yellow;

    private Color initialColor;


    protected Rigidbody rb;

    protected Renderer colorRenderer;


    protected bool caught = false;


    public virtual void Start()
    {

        networkObject = GetComponent<NetworkObject>();

        colorRenderer = GetComponentInChildren<Renderer>();

        initialColor = colorRenderer.material.color;

        rb = GetComponent<Rigidbody>();

    }


    void Update()
    {



    }


    public virtual void LocalCatch()
    {
        print("LocalCatch");
        if (!caught)
        { 
            if (!IsOwner)
            {
                RequestOwnershipRpc();
            }
            Catch();
        }
    }

    public virtual void Catch()
    {
        print("Catch");
        rb.isKinematic = true;
        caught = true;
        ShowCaughtRpc();

    }


    public void ShowCaught()
    {

        print("ShowCaught");

        colorRenderer.material.color = caughtColor;

    }


    public virtual void LocalRelease()
    {

        print("LocalRelease");

        Release();

    }


    public virtual void Release()
    {

        print("Release");

        rb.isKinematic = false;

        caught = false;

        ShowReleasedRpc();

    }


    public void ShowReleased()
    {
        print("ShowReleased");
        colorRenderer.material.color = catchableColor;
    }


    public void LocalShowCatchable()
    {

        print("LocalShowCatchable");
        ShowCatchableRpc();

    }



    public void ShowCatchable()
    {
        colorRenderer.material.color = catchableColor;
    }



    public void LocalHideCatchable()
    {
        print("LocalHideCatchable");
        HideCatchableRpc();
    }


    public void HideCatchable()
    {

        colorRenderer.material.color = initialColor;

    }


    // --- APPELS RÉSEAU (RPCs) ---

    [Rpc(SendTo.Server)]
    private void RequestOwnershipRpc(RpcParams rpcParams = default)
    {
        networkObject.ChangeOwnership(rpcParams.Receive.SenderClientId);
    }

    [Rpc(SendTo.Everyone)]
    public void ShowCaughtRpc()
    {
        print("ShowCaughtRpc (Network)");
        ShowCaught();
    }

    [Rpc(SendTo.Everyone)]
    public void ShowReleasedRpc()
    {
        print("ShowReleasedRpc (Network)");
        ShowReleased();
    }

    [Rpc(SendTo.Everyone)]
    public void ShowCatchableRpc()
    {
        print("ShowCatchableRpc (Network)");
        ShowCatchable();
    }

    [Rpc(SendTo.Everyone)]
    public void HideCatchableRpc()
    {
        print("HideCatchableRpc (Network)");
        HideCatchable();
    }

}