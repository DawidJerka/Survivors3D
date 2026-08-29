using System.Collections.Generic;
using UnityEngine;

public class OrbitingBladesWeapon : Weapon
{
    [SerializeField] private OrbitingBlade bladePrefab;

    private OrbitingBladesData bladesData;

    private readonly List<OrbitingBlade> activeBlades = new();

    protected override void OnInitialized()
    {
        bladesData = Data as OrbitingBladesData;

        if (bladesData == null)
        {
            Debug.LogError(
                "OrbitingBladesWeapon requires OrbitingBladesData.",
                this
            );

            return;
        }

        RebuildBlades();
    }

    private void Update()
    {
        if (!IsInitialized || bladesData == null)
            return;

        transform.Rotate(
            0f,
            bladesData.RotationSpeed * Time.deltaTime,
            0f,
            Space.Self
        );
    }

    public override void HandleLevelChanged(int newLevel)
    {
        if (bladesData == null)
            return;

        RebuildBlades();
    }

    private void RebuildBlades()
    {
        ClearBlades();

        int bladeCount =
            bladesData.GetBladeCount(CurrentLevel);

        for (int i = 0; i < bladeCount; i++)
        {
            float angle =
                (360f / bladeCount) * i;

            float angleRadians =
                angle * Mathf.Deg2Rad;

            Vector3 localPosition = new Vector3(
                Mathf.Cos(angleRadians) * bladesData.OrbitRadius,
                bladesData.OrbitHeight,
                Mathf.Sin(angleRadians) * bladesData.OrbitRadius
            );

            OrbitingBlade blade = Instantiate(
                bladePrefab,
                transform
            );

            blade.transform.localPosition = localPosition;

            blade.transform.localRotation =
                Quaternion.Euler(0f, -angle, 0f);

            blade.Initialize(bladesData.Damage);

            activeBlades.Add(blade);
        }
    }

    private void ClearBlades()
    {
        foreach (OrbitingBlade blade in activeBlades)
        {
            if (blade != null)
            {
                Destroy(blade.gameObject);
            }
        }

        activeBlades.Clear();
    }
}