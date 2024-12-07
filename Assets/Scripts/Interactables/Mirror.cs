using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Mirror : MonoBehaviour, IInteractable
{
    public GameObject player;
    public float offset = 2f;

    private Camera mirrorCamera;
    private RenderTexture _renderTexture;
    [SerializeField] private Material _material;

    private static int instanceCount = 0; // Contador para garantir nomes únicos
    private string renderTexturePath;

    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;

        player = FindFirstObjectByType<PlayerController>().gameObject;

        mirrorCamera = GetComponentInChildren<Camera>();
        if (mirrorCamera == null)
        {
            GameObject cameraObject = new GameObject("Mirror Camera");
            cameraObject.transform.SetParent(transform);
            mirrorCamera = cameraObject.AddComponent<Camera>();
            mirrorCamera.transform.localPosition = Vector3.zero;
            mirrorCamera.transform.localRotation = Quaternion.identity;
        }

        CreateAndSaveRenderTexture();

        mirrorCamera.targetTexture = _renderTexture;
    
        // Assign the RenderTexture to the prefab's material
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            _material.SetTexture("_Texture2D", _renderTexture); // Assign the RenderTexture to the "_BaseMap" property
            _material.SetFloat("_Flip", 1); // Assign the RenderTexture to the "_BaseMap" property
            meshRenderer.material = _material;
        }
        else
        {
            Debug.LogWarning("No MeshRenderer found on the prefab. RenderTexture assignment skipped.");
        }
    }

    private void CreateAndSaveRenderTexture()
    {
        // Liberar memória anterior (se necessário)
        if (_renderTexture != null)
        {
            _renderTexture.Release();
            _renderTexture = null;
        }

        // Incrementar contador para nomes únicos
        instanceCount++;
        string uniqueName = $"Mirror_RenderTexture_{instanceCount}";
        renderTexturePath = $"Assets/RenderTextures/{uniqueName}.renderTexture";

        // Criar RenderTexture
        _renderTexture = new RenderTexture(256, 256, 16);
        _renderTexture.name = uniqueName;

        // Salvar RenderTexture como ativo no projeto
        if (!AssetDatabase.IsValidFolder("Assets/RenderTextures"))
        {
            AssetDatabase.CreateFolder("Assets", "RenderTextures");
        }

        AssetDatabase.CreateAsset(_renderTexture, renderTexturePath);
        AssetDatabase.SaveAssets();

        Debug.Log($"RenderTexture salva em: {renderTexturePath}");
    }

    public void Interact(Vector3 hitDirection)
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 5)
        {
            Vector3 dynamicOffset = -hitDirection.normalized * offset;
            player.transform.position = transform.position + dynamicOffset;
        }
    }
}
