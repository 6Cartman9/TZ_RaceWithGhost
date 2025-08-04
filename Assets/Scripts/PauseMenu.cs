using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseBanner;

    [Header("—цены")]
    public string mainMenuSceneName = "MainMenu";

    [Header("јудио")]
    public AudioMixer audioMixer;
    public string exposedVolumeParam = "MasterVolume";

    // внутреннее
    private bool isPaused = false;
    private float previousListenerVolume = 1f;
    private float previousMixerVolume = 0f; 
    void Start()
    {
        if (pauseBanner != null)
            pauseBanner.SetActive(false);
        previousListenerVolume = AudioListener.volume;

        if (audioMixer != null && !string.IsNullOrEmpty(exposedVolumeParam))
        {
            if (!audioMixer.GetFloat(exposedVolumeParam, out previousMixerVolume))
                previousMixerVolume = 0f;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;

        Time.timeScale = 0f;

        previousListenerVolume = AudioListener.volume;
        AudioListener.volume = 0f;

        if (audioMixer != null && !string.IsNullOrEmpty(exposedVolumeParam))
        {
            audioMixer.SetFloat(exposedVolumeParam, -80f);
        }

        if (pauseBanner != null)
            pauseBanner.SetActive(true);
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;

        Time.timeScale = 1f;

        AudioListener.volume = previousListenerVolume;

        if (audioMixer != null && !string.IsNullOrEmpty(exposedVolumeParam))
        {
            audioMixer.SetFloat(exposedVolumeParam, previousMixerVolume);
        }

        if (pauseBanner != null)
            pauseBanner.SetActive(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioListener.volume = previousListenerVolume;
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
        else
        AudioListener.volume = previousListenerVolume;
    }
}
