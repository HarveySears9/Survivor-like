using UnityEngine;
using UnityEngine.UI;

public class ScrollViewScaler : MonoBehaviour
{
    public ScrollRect scrollRect; // Reference to the ScrollRect
    public RectTransform content; // Content RectTransform
    public RectTransform viewport; // Viewport RectTransform
    public float scaleMultiplier = 1.5f; // Max scale factor
    public float scaleDistance = 200f; // Distance from center where scaling starts

    void Update()
    {
        // Loop through each child of the content
        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform item = content.GetChild(i).GetComponent<RectTransform>();

            // Calculate the distance from the item's center to the viewport's center
            Vector3 viewportCenter = viewport.position;
            Vector3 itemCenter = item.position;

            float distance = Vector3.Distance(viewportCenter, itemCenter);

            // Calculate a scale factor based on the distance
            float scale = Mathf.Lerp(scaleMultiplier, 1f, distance / scaleDistance);
            scale = Mathf.Clamp(scale, 1f, scaleMultiplier);

            // Apply the scale to the item
            item.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
