using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narration : MonoBehaviour
{
    public AudioClip audio;
    PlayerController player;
    List<Transform> pathPoints = new List<Transform>();


    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        //audio = GetComponent<AudioSource>().GetComponent<AudioClip>();
        pathPoints = player.GetPathPoints();

        float time = audio.length;
        float pathLength = 0;

        for (int i = 0; i < pathPoints.Count - 2; i++)
        {
            pathLength += Vector2.Distance(pathPoints[i].position, pathPoints[i + 1].position);
        }

        float playerSpeed = pathLength / time;

        player.SetSpeed(playerSpeed);
        GetComponent<AudioSource>().Play();
    }
}
