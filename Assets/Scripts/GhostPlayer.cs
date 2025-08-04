using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class GhostPlayer: MonoBehaviour
{
    private List<Vector3> path = new();
    private List<Quaternion> rotations = new();
    private int index = 0;
    private bool playing = false;

    [Header("Настройки скорости")]
    public float moveSpeed = 10f;
    public float rotationLerpSpeed = 8f;
    public float pointThreshold = 0.05f;

    public void Init(IReadOnlyList<Vector3> recordedPositions, IReadOnlyList<Quaternion> recordedRotations)
    {
        if (recordedPositions == null || recordedRotations == null) return;
        if (recordedPositions.Count == 0 || recordedPositions.Count != recordedRotations.Count) return;

        path = new List<Vector3>(recordedPositions);
        rotations = new List<Quaternion>(recordedRotations);

        index = 0;
        transform.position = path[0];
        transform.rotation = rotations[0];
        playing = true;
    }

    void Update()
    {
        if (!playing || index >= path.Count - 1) return;

        Vector3 targetPos = path[index + 1];
        Vector3 direction = (targetPos - transform.position);
        float distanceToTarget = direction.magnitude;

        if (distanceToTarget < pointThreshold)
        {
            index++;
            if (index >= path.Count - 1)
            {
                playing = false;
                return;
            }
            targetPos = path[index + 1];
            direction = (targetPos - transform.position);
            distanceToTarget = direction.magnitude;
        }

        Vector3 moveDir = direction.normalized;
        float moveStep = moveSpeed * Time.deltaTime;

        // Двигаемся с фиксированной скоростью
        if (moveStep >= distanceToTarget)
        {
            transform.position = targetPos;
            index++;
        }
        else
        {
            transform.position += moveDir * moveStep;
        }

        // Плавно ориентируемся на ротацию соответствующей точки
        Quaternion targetRot = rotations[Mathf.Clamp(index + 1, 0, rotations.Count - 1)];
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerpSpeed * Time.deltaTime);
    }
}
