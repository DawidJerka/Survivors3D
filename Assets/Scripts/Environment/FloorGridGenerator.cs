using UnityEngine;

public class FloorGridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private int gridSize = 8;
    [SerializeField] private float tileSpacing = 10f;

    [ContextMenu("Generate Floor")]
    public void Generate()
    {
        Clear();

        float offset =
            (gridSize - 1) * tileSpacing / 2f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(
                    x * tileSpacing - offset,
                    -0.045f,
                    z * tileSpacing - offset
                );

                GameObject tile = Instantiate(
                    tilePrefab,
                    transform
                );

                tile.transform.localPosition = position;
                tile.transform.localRotation = Quaternion.identity;

                tile.name = $"FloorTile_{x}_{z}";
            }
        }
    }

    [ContextMenu("Clear Floor")]
    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(
                transform.GetChild(i).gameObject
            );
        }
    }
}