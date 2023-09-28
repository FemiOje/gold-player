using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Hertzole.GoldPlayer; //the namespace for Gold Player Input System

public class VideoPlayerController : MonoBehaviour
{
    public GameObject goldPlayerPrefab;
    VideoPlayer exampleVideo;
    bool triggerEntered;
    bool videoPlaying;
    public GameObject instructionText;


    // Start is called before the first frame update
    void Start()
    {
        goldPlayerPrefab = GameObject.Find("Gold Player Prefab");
        exampleVideo = GameObject.Find("Video Surface").GetComponent<VideoPlayer>();
        triggerEntered = false;
        videoPlaying = false;
        instructionText.SetActive(true);

        // Subscribe to video events
        exampleVideo.prepareCompleted += OnVideoPrepareCompleted;
        exampleVideo.loopPointReached += OnVideoLoopPointReached;
    }

    // Update is called once per frame
    void Update()
    {
        if (exampleVideo.isPlaying)
        {
            videoPlaying = true;
        }
        else
        {
            videoPlaying = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerEntered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        triggerEntered = false;
    }

    public void PlayVideo()
    {
        GoldPlayerInputSystem goldPlayerControls = goldPlayerPrefab.GetComponent<GoldPlayerInputSystem>();
        if (triggerEntered && !videoPlaying)
        {
            // Deactivate the specified component while the video is playing
            if (goldPlayerControls != null)
            {
                goldPlayerControls.enabled = false;
            }

            instructionText.SetActive(false); // Deactivate instructionText
            //goldPlayerPrefab.transform.position = watchVideoPosition; //Vector3(2.3873024,0.079999879,-11.0638742)
            //goldPlayerPrefab.transform.LookAt(videoPlayerBox.transform);
            exampleVideo.Play();
        }
    }

    // Called when the video is prepared and ready to play
    private void OnVideoPrepareCompleted(VideoPlayer source)
    {
        // The video is ready to play
    }

    // Called when the video reaches its end (including looping)
    private void OnVideoLoopPointReached(VideoPlayer source)
    {
        GoldPlayerInputSystem goldPlayerControls = goldPlayerPrefab.GetComponent<GoldPlayerInputSystem>();
        // Reactivate the specified component after the video finishes playing
        if (goldPlayerControls != null)
        {
            goldPlayerControls.enabled = true;
        }

        // Deactivate the video and reactivate instructionText
        exampleVideo.Stop();
        instructionText.SetActive(true); // Reactivate instructionText
    }

    // Unsubscribe from events when the script is destroyed
    private void OnDestroy()
    {
        exampleVideo.prepareCompleted -= OnVideoPrepareCompleted;
        exampleVideo.loopPointReached -= OnVideoLoopPointReached;
    }
}
