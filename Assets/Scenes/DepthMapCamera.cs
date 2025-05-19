using UnityEngine;

public class DepthMapCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Camera depthCamera;
    
    public Material depthMaterial;
    public RenderTexture depthRenderTexture;
    
    [Range(1, 1000)]
    public float depthScale = 100f;
    
    void Start()
    {
        // Create the depth camera if not assigned
        if (depthCamera == null)
        {
            GameObject depthCamObj = new GameObject("DepthCamera");
            depthCamObj.transform.parent = mainCamera.transform;
            depthCamObj.transform.localPosition = Vector3.zero;
            depthCamObj.transform.localRotation = Quaternion.identity;
            
            depthCamera = depthCamObj.AddComponent<Camera>();
        }
        
        // Create render texture if not assigned
        if (depthRenderTexture == null)
        {
            depthRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            depthRenderTexture.name = "DepthRenderTexture";
        }
        
        // Configure depth camera
        depthCamera.CopyFrom(mainCamera);
        depthCamera.clearFlags = CameraClearFlags.SolidColor;
        depthCamera.backgroundColor = Color.white;
        depthCamera.targetTexture = depthRenderTexture;
        
        // Use the depth material for rendering
        depthCamera.SetReplacementShader(Shader.Find("Custom/DepthShader"), "RenderType");
    }
    
    void Update()
    {
        // Keep the same properties as main camera
        depthCamera.fieldOfView = mainCamera.fieldOfView;
        
        // Update the depth scale in the material
        if (depthMaterial != null)
        {
            depthMaterial.SetFloat("_DepthScale", depthScale);
        }
    }
    
    void OnGUI()
    {
        // Display the depth map in a corner of the screen (for debugging)
        float size = Screen.height / 4;
        GUI.DrawTexture(new Rect(Screen.width - size, 0, size, size), depthRenderTexture);
    }
}