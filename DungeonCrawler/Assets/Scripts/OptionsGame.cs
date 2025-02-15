using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class OptionsGame : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;

    private List<Resolution> uniqueResolutions = new List<Resolution>();

    void Start()
    {
        Debug.Log("Options menu initialized");

        Resolution[] resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions(); // Clear existing options

        List<string> options = new List<string>();
        HashSet<string> uniqueResSet = new HashSet<string>();

        int currentResolutionIndex = 0;
        int optionIndex = 0;

        foreach (Resolution res in resolutions)
        {
            string option = res.width + "x" + res.height;
            if (uniqueResSet.Add(option)) // Only add unique resolutions
            {
                options.Add(option);
                uniqueResolutions.Add(res);

                // Use Screen.width & Screen.height instead of Screen.currentResolution
                if (res.width == Screen.width && res.height == Screen.height)
                {
                    currentResolutionIndex = optionIndex;
                }
                optionIndex++;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void setResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < uniqueResolutions.Count)
        {
            Resolution resolution = uniqueResolutions[resolutionIndex];

            // Use coroutine to ensure resolution change applies even when paused
            StartCoroutine(ApplyResolution(resolution.width, resolution.height));
        }
    }

    private IEnumerator ApplyResolution(int width, int height)
    {
        yield return new WaitForEndOfFrame(); // Wait to ensure resolution is applied properly
        Screen.SetResolution(width, height, Screen.fullScreenMode);
        Debug.Log($"Resolution set to: {width}x{height}");
    }

    public void setFullscreen(bool isFullscreen)
    {
        FullScreenMode mode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

        // Apply fullscreen mode explicitly with resolution settings
        StartCoroutine(ApplyFullscreen(mode));
    }

    private IEnumerator ApplyFullscreen(FullScreenMode mode)
    {
        yield return new WaitForEndOfFrame();
        Screen.fullScreenMode = mode;
        Screen.SetResolution(Screen.width, Screen.height, mode);
        Debug.Log($"Fullscreen mode set to: {mode}");
    }
}
