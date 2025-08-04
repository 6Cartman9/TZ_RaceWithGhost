using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
    public GameObject playerCarPrefab;
    public GameObject ghostCarPrefab;
    public Transform spawnPoint;
    public Text lapText;
    public GameObject statusText;
    public GameObject finishBanner;
    public LapTrigger lapTrigger;

    private GameObject playerInstance;
    private GameObject ghostInstance;
    private GhostRecorder recorder;
    private GhostPlayer ghostPlayer;

    private bool firstLapDone = false;
    private bool secondLapDone = false;
    private bool waitingForEnter = true;

    void Start()
    {
        Time.timeScale = 0f;
        SpawnPlayer();

        if (lapTrigger == null)
        {
            Debug.LogError("lapTrigger не назначен!");
        }
        else
        {
            lapTrigger.onFirstLapComplete.AddListener(OnFirstLapCompleted);
            lapTrigger.onSecondLapComplete.AddListener(OnSecondLapCompleted);
        }

        UpdateLapUI();
        finishBanner.SetActive(false);
    }

    void Update()
    {
        if (waitingForEnter)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                waitingForEnter = false;
                Time.timeScale = 1f;
                statusText.SetActive(false);
                recorder.StartRecording();
            }

            return;
        }

        UpdateLapUI();
    }

    void SpawnPlayer()
    {
        if (playerInstance != null) Destroy(playerInstance);
        playerInstance = Instantiate(playerCarPrefab, spawnPoint.position, spawnPoint.rotation);
        playerInstance.tag = "Player";
        recorder = playerInstance.GetComponent<GhostRecorder>();
        
    }

    public void OnFirstLapCompleted()
    {
        if (firstLapDone) return;
        firstLapDone = true;

        recorder.StopRecording();

        ghostInstance = Instantiate(ghostCarPrefab, spawnPoint.position, spawnPoint.rotation);
        ghostPlayer = ghostInstance.GetComponent<GhostPlayer>();
       
        var ghostPlayerFixed = ghostInstance.GetComponent<GhostPlayer>();
        ghostPlayerFixed.Init(recorder.RecordedPositions, recorder.RecordedRotations);

    }

    public void OnSecondLapCompleted()
    {
        if (secondLapDone) return;
        secondLapDone = true;

        Time.timeScale = 0f;
        finishBanner.SetActive(true);
    }

    void UpdateLapUI()
    {
        if (lapText == null || lapTrigger == null) return;

        int displayedLap = lapTrigger.CurrentLap + 1;
        if (displayedLap > 2) displayedLap = 2; 

        lapText.text = $"Laps: {displayedLap} / 2";
    }
}
