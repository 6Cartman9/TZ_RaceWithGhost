using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class LapTrigger : MonoBehaviour
{
    public string playerTag = "Player";
    public int requiredLaps = 2;
    public UnityEvent onFirstLapComplete;
    public UnityEvent onSecondLapComplete;

    private int currentLap = 0;
    private bool canCount = true;

    private void Awake()
    {
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (!canCount) return;

        currentLap++;
        canCount = false;

        if (currentLap == 1)
            onFirstLapComplete?.Invoke();
        else if (currentLap == requiredLaps)
            onSecondLapComplete?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            canCount = true;
        }
    }

    public int CurrentLap => currentLap;
}
