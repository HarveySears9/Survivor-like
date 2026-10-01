using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("Meteor")]
    public float fallSpeed = 10f;

    [Header("Damage")]
    public float damage = 1f;

    [Header("Impact")]
    public GameObject aoePrefab;
    public GameObject shadowPrefab;

    [Header("Area Size")]
    [HideInInspector]
    public float areaSizeMultiplier = 1f;

    [HideInInspector]
    public Vector3 targetPosition;

    private GameObject shadowInstance;

    private float startY;

    private Vector3 shadowStartScale;

    private Vector3 meteorStartScale;

    void Start()
    {
        meteorStartScale =
            transform.localScale;

        Vector3 shadowPos = targetPosition;
        shadowPos.y -= 0.25f;

        shadowInstance =
            Instantiate(
                shadowPrefab,
                shadowPos,
                Quaternion.identity
            );

        shadowStartScale =
            shadowInstance.transform.localScale;

        shadowStartScale *= areaSizeMultiplier;

        shadowInstance.transform.localScale =
            shadowStartScale * 0.1f;

        transform.localScale =
            meteorStartScale * areaSizeMultiplier;

        startY =
            transform.position.y;
    }

    void Update()
    {
        float step =
            fallSpeed * Time.deltaTime;

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                targetPosition,
                step
            );

        // Grow the shadow as the meteor approaches
        if (shadowInstance != null)
        {
            float progress =
                1f -
                (transform.position.y - targetPosition.y) /
                (startY - targetPosition.y);

            progress =
                Mathf.Clamp01(progress);

            shadowInstance.transform.localScale =
                Vector3.Lerp(
                    shadowStartScale * 0.1f,
                    shadowStartScale,
                    progress
                );
        }

        // Check if the meteor has landed
        if (Vector3.Distance(
                transform.position,
                targetPosition
            ) < 0.1f)
        {
            Impact();
        }
    }

    void Impact()
    {
        Vector3 aoePos =
            transform.position;

        aoePos.y -= 0.1f;

        GameObject aoe =
            Instantiate(
                aoePrefab,
                aoePos,
                Quaternion.identity
            );

        aoe.transform.localScale *= areaSizeMultiplier;

        DamageOverTimeArea damageArea =
            aoe.GetComponent<DamageOverTimeArea>();

        if (damageArea != null)
        {
            damageArea.SetDamage(damage);
        }

        if (shadowInstance != null)
        {
            Destroy(shadowInstance);
        }

        Destroy(gameObject);
    }
}