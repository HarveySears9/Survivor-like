using System.Collections.Generic;
using UnityEngine;

public class Magnet : EquipmentBase
{
    [Header("Magnet")]
    public float range = 0.5f;

    protected override void Start()
    {
        base.Start();

        range = PlayerStats.GetPickupRadius();
    }

    protected override void ApplyLevel()
    {
        switch (level)
        {
            case 1:
                range =
                    PlayerStats.GetPickupRadius() * 1.5f;
                break;

            case 2:
                range =
                    PlayerStats.GetPickupRadius() * 2f;
                break;

            case 3:
                range =
                    PlayerStats.GetPickupRadius() * 2.5f;
                break;

            case 4:
                range =
                    PlayerStats.GetPickupRadius() * 3f;
                break;

            case 5:
                range =
                    PlayerStats.GetPickupRadius() * 3.5f;
                break;
        }
    }

    private void FixedUpdate()
    {
        AttractCollectibles();
    }

    private void AttractCollectibles()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(
                transform.position,
                range
            );

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Item"))
            {
                collider.transform.position =
                    Vector3.MoveTowards(
                        collider.transform.position,
                        transform.position,
                        5f * Time.deltaTime
                    );
            }
        }
    }
}