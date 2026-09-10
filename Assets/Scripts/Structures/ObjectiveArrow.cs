using UnityEngine;

public class ObjectiveArrow : MonoBehaviour
{
    [SerializeField] private RectTransform arrowRect;
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float hideDistance = 2.5f;
    [SerializeField] private float arrowRadius = 120f;
    [SerializeField] private float minRadius = 60f;

    [Header("Fade")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeStartDistance = 6f;

    private Transform target;
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;

        arrowRect.gameObject.SetActive(false);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        arrowRect.gameObject.SetActive(true);
    }

    public void HideArrow()
    {
        target = null;

        canvasGroup.alpha = 0f;

        arrowRect.gameObject.SetActive(false);
    }

    void Update()
    {
        if (target == null)
        {
            arrowRect.gameObject.SetActive(false);
            return;
        }

        float distance =
            Vector2.Distance(
                player.position,
                target.position
            );

        // Hide when player gets close enough
        if (distance <= hideDistance)
        {
            canvasGroup.alpha = 0f;

            arrowRect.gameObject.SetActive(false);

            target = null;

            return;
        }

        arrowRect.gameObject.SetActive(true);

        UpdateArrow();

        UpdateFade(distance);
    }

    void UpdateArrow()
    {
        Vector3 screenPos =
            cam.WorldToScreenPoint(
                target.position
            );

        // Handle target behind camera
        if (screenPos.z < 0)
            screenPos *= -1;

        Vector2 screenCenter =
            new Vector2(
                Screen.width / 2f,
                Screen.height / 2f
            );

        Vector2 dir =
            (
                (Vector2)screenPos -
                screenCenter
            ).normalized;

        // Rotate arrow
        float angle =
            Mathf.Atan2(
                dir.y,
                dir.x
            ) * Mathf.Rad2Deg;

        arrowRect.rotation =
            Quaternion.Euler(
                0,
                0,
                angle - 90f
            );

        // Distance-based radius
        float distance =
            Vector2.Distance(
                player.position,
                target.position
            );

        float t =
            Mathf.InverseLerp(
                hideDistance,
                fadeStartDistance,
                distance
            );

        float dynamicRadius =
            Mathf.Lerp(
                minRadius,
                arrowRadius,
                t
            );

        // Position arrow
        arrowRect.position =
            screenCenter +
            dir * dynamicRadius;
    }

    void UpdateFade(float distance)
    {
        float alpha =
            Mathf.InverseLerp(
                hideDistance,
                fadeStartDistance,
                distance
            );

        canvasGroup.alpha =
            Mathf.Clamp01(alpha);
    }
}