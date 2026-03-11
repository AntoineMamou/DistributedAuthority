using Unity.Netcode.Components;
using UnityEngine;

#if UNITY_EDITOR
using Unity.Netcode.Editor;
using UnityEditor;

/// <summary>
/// The custom editor for the <see cref="PlayerController"/> component.
/// </summary>
[CustomEditor(typeof(PlayerController), true)]
public class PlayerControllerEditor : NetworkTransformEditor
{
    private SerializedProperty m_Speed;
    private SerializedProperty m_ApplyVerticalInputToZAxis;

    public override void OnEnable()
    {
        m_Speed = serializedObject.FindProperty(nameof(PlayerController.Speed));
        m_ApplyVerticalInputToZAxis = serializedObject.FindProperty(nameof(PlayerController.ApplyVerticalInputToZAxis));
        base.OnEnable();
    }

    private void DisplayPlayerControllerProperties()
    {
        EditorGUILayout.PropertyField(m_Speed);
        EditorGUILayout.PropertyField(m_ApplyVerticalInputToZAxis);
    }

    public override void OnInspectorGUI()
    {
        var playerController = target as PlayerController;
        void SetExpanded(bool expanded) { playerController.PlayerControllerPropertiesVisible = expanded; }

        DrawFoldOutGroup<PlayerController>(playerController.GetType(), DisplayPlayerControllerProperties, playerController.PlayerControllerPropertiesVisible, SetExpanded);
        base.OnInspectorGUI();
    }
}
#endif

public class PlayerController : NetworkTransform
{
#if UNITY_EDITOR
    public bool PlayerControllerPropertiesVisible;
#endif
    public float Speed = 10;

    // Note : Cette variable n'est plus forcément utile si on utilise Translate/Rotate, 
    // mais je la laisse pour ne pas casser votre Inspecteur.
    public bool ApplyVerticalInputToZAxis = true;

    public Vector3 cameraPositionOffset = new Vector3(0, 1.6f, 0);
    public Quaternion cameraOrientationOffset = new Quaternion();
    protected Transform cameraTransform;
    protected Camera theCamera;

    private void Start()
    {
        CatchCamera();
    }

    private void Update()
    {
        // 1. Si ce n'est pas notre joueur, on ne met pas à jour.
        if (!IsSpawned || !IsOwner)
        {
            return;
        }

        // 2. CORRECTION DU FOCUS : On ignore les inputs si la fenêtre n'est pas active.
        if (!Application.isFocused)
        {
            return;
        }

        if (!Input.GetKey(KeyCode.LeftAlt)) return; 
        

        // 3. CORRECTION DU MOUVEMENT : On évite de faire le mouvement deux fois.
        // J'ai gardé votre logique de "Tank" (tourner avec gauche/droite, avancer avec haut/bas).

        float rotationInput = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float forwardInput = Input.GetAxis("Vertical") * Time.deltaTime * Speed;

        // Applique la rotation sur l'axe Y
        transform.Rotate(0, rotationInput, 0);

        // Applique le mouvement vers l'avant (axe Z local)
        transform.Translate(0, 0, forwardInput);
    }

    public void CatchCamera()
    {

        if (IsSpawned && HasAuthority)
        {

            // attach the camera to the navigation rig

            theCamera = (Camera)GameObject.FindFirstObjectByType(typeof(Camera));

            theCamera.enabled = true;

            cameraTransform = theCamera.transform;

            cameraTransform.SetParent(transform);

            cameraTransform.localPosition = cameraPositionOffset;

            cameraTransform.localRotation = cameraOrientationOffset;

        }

    }
}