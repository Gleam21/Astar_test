using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int numOfRows;
    public int numOfColumns;
    public float cellSize;

    private void OnDrawGizmos()
    {
        float width = (numOfColumns * cellSize);
        float height = (numOfRows * cellSize);

        for (int i = 0; i < numOfRows + 1; i++)
        {
            Vector3 startPosition = transform.position + i * cellSize * new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 endPosition = startPosition + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPosition, endPosition, Color.green);
        }

        for (int i = 0; i < numOfColumns + 1; i++)
        {
            Vector3 startPosition = transform.position + i * cellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPosition = startPosition + height * new Vector3(0.0f, 0.0f, 1.0f);
            Debug.DrawLine(startPosition, endPosition, Color.green);
        }
    }
}
