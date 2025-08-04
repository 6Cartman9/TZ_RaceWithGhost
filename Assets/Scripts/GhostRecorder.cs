using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    public float recordInterval = 0.02f; // частота записи
    private float timer;
    private bool recording = false;

    private List<Vector3> positions = new();
    private List<Quaternion> rotations = new();
    private List<float> timestamps = new(); // время с начала записи

    private float elapsed = 0f;

    public IReadOnlyList<Vector3> RecordedPositions => positions;
    public IReadOnlyList<Quaternion> RecordedRotations => rotations;
    public IReadOnlyList<float> RecordedTimestamps => timestamps;

    public void StartRecording()
    {
        positions.Clear();
        rotations.Clear();
        timestamps.Clear();
        timer = 0f;
        elapsed = 0f;
        recording = true;
        RecordFrame(); 
    }

    public void StopRecording()
    {
        recording = false;
    }

    void Update()
    {
        if (!recording) return;

        elapsed += Time.deltaTime;
        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            RecordFrame();
            timer = 0f;
        }
    }

    private void RecordFrame()
    {
        positions.Add(transform.position);
        rotations.Add(transform.rotation);
        timestamps.Add(elapsed);
    }
}
