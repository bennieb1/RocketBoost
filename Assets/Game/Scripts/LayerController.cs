using UnityEngine;

public class LayerController : MonoBehaviour
{
    public LayerMask transparentLayer; // Assign this in the Inspector
    public float distanceThreshold = 5f; // Distance at which transparency changes
    public Transform player; // The player's or camera's position

    private void Update()
    {
        // Find all objects with a Renderer component in the scene, including inactive ones, and sort them
        Renderer[] renderers = GameObject.FindObjectsByType<Renderer>(FindObjectsSortMode.None);

        foreach (Renderer renderer in renderers)
        {
            GameObject obj = renderer.gameObject;
            int objLayer = obj.layer;

            // Check if the object's layer is in the transparentLayer LayerMask
            if ((transparentLayer & (1 << objLayer)) != 0)
            {
                float distance = Vector3.Distance(player.position, obj.transform.position);
                Material material = renderer.material;

                if (material != null)
                {
                    // Use MaterialPropertyBlock for efficiency
                    MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                    renderer.GetPropertyBlock(materialPropertyBlock);

                    Color color = materialPropertyBlock.GetColor("_Color");

                    // Fade based on distance
                    if (distance < distanceThreshold)
                    {
                        float transparency = Mathf.Clamp01(distance / distanceThreshold);
                        color.a = transparency;
                    }
                    else
                    {
                        color.a = 1f; // Fully opaque when far away
                    }

                    materialPropertyBlock.SetColor("_Color", color);
                    renderer.SetPropertyBlock(materialPropertyBlock);
                }
            }
        }
    }
}