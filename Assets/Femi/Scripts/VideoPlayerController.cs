using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Hertzole.GoldPlayer; //the namespace for Gold Player Input System

public class VideoPlayerController : MonoBehaviour
{
    public GameObject goldPlayerPrefab;

    VideoPlayer worldSpaceVideo;
    VideoPlayer cameraSpaceVideo;

    bool triggerEntered;
    bool isVideoPlaying;

    public GameObject instructionText;

    [SerializeField] GameObject playerCamera;
    string triggerName;


    // Start is called before the first frame update
    void Start()
    {
        goldPlayerPrefab = GameObject.Find("Gold Player Prefab");

        worldSpaceVideo = GameObject.Find("World Space Video Surface").GetComponent<VideoPlayer>();
        cameraSpaceVideo = playerCamera.GetComponent<VideoPlayer>();

        triggerEntered = false;
        isVideoPlaying = false;

        instructionText.SetActive(true);

        // Subscribe to video events
        worldSpaceVideo.prepareCompleted += OnVideoPrepareCompleted;
        worldSpaceVideo.loopPointReached += OnVideoLoopPointReached;

        cameraSpaceVideo.prepareCompleted += OnVideoPrepareCompleted;
        cameraSpaceVideo.loopPointReached += OnVideoLoopPointReached;
    }

    // Update is called once per frame
    void Update()
    {
        if ((worldSpaceVideo != null && worldSpaceVideo.isPlaying) || (cameraSpaceVideo != null && cameraSpaceVideo.isPlaying))
        {
            isVideoPlaying = true;
            instructionText.SetActive(false); // deactivate instructionText
        }
        else
        {
            isVideoPlaying = false;
            instructionText.SetActive(true); // Reactivate instructionText
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerEntered = true;

        if (gameObject.tag.Equals("Camera Space"))
        {
            triggerName = "Camera Space";

        } else if (gameObject.tag.Equals("World Space"))
        {
            triggerName = "World Space";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        triggerEntered = false;
        triggerName = "None";
    }

    public void PlayVideo()
    {
        GoldPlayerInputSystem goldPlayerControls = goldPlayerPrefab.GetComponent<GoldPlayerInputSystem>();

        if (triggerEntered && !isVideoPlaying)
        {
            // Deactivate the specified component while the video is playing
            if (goldPlayerControls != null)
            {
                goldPlayerControls.enabled = false;
            }

            //instructionText.SetActive(false); // Deactivate instructionText

            if (triggerName == "World Space")
            {
                worldSpaceVideo.Play();
            }
            else if (triggerName == "Camera Space")
            {
                cameraSpaceVideo.Play();
            }
        }
    }

    // Called when the video is prepared and ready to play
    private void OnVideoPrepareCompleted(VideoPlayer source)
    {
        // The video is ready to play
        source.Play();
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

        // Deactivate the video
        source.Stop();
    }

    // Unsubscribe from events when the script is destroyed
    private void OnDestroy()
    {
        worldSpaceVideo.prepareCompleted -= OnVideoPrepareCompleted;
        worldSpaceVideo.loopPointReached -= OnVideoLoopPointReached;

        cameraSpaceVideo.prepareCompleted -= OnVideoPrepareCompleted;
        cameraSpaceVideo.loopPointReached -= OnVideoLoopPointReached;
    }
}
