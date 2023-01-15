using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallibrationSettings : MonoBehaviour
{
    [Header("Sensor Settings")]
    public bool IMU = true;
    public bool VideoCam = false;
    public bool HeartRate = false;
    public bool SpO2 = false;

    [Header("Other Settings")]
    public bool FootDrop = true;
    public bool Speed = true;
    public bool Distance = false;

    public Inputy Input;
    public Outputy Output;

    public GameObject SceneManager;

    [ContextMenu("Configure Project")]
    public void DoSomething()
    {
        SceneManager.SetActive(true);
        Debug.Log("Project Configured!!");
    }

    public enum Inputy
    {
        Angle,
        GaitVelocity,
        DistanceTravelled
    }

    public enum Outputy
    {
        FootDropLikelihood,
        DeltaIntensity
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
