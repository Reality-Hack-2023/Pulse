using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject status0;
    public GameObject status1;
    public GameObject status2;
    public GameObject status3;
    public GameObject status4;
    public GameObject status5;
    int status = 0;

    /**
     * status = 0  Init scene, start calibration
     * trigger switch
     * status = 1 walking stage, go walking
     * trigger collision
     * status = 2 reached and completed calibration, 
     * trigger switch
     * status = 3 init scene for assessment
     * trigger switch
     * status = 4 walking stage go walking
     * trigger collision
     * status = 5 reached and completed assessment
     * trigger switch
     * status = 6 which is 0
     * 
     */

    void disableall()
    {
        status0.SetActive(false);
        status1.SetActive(false);
        status2.SetActive(false);
        status3.SetActive(false);
        status4.SetActive(false);
        status5.SetActive(false);
    }

    void enablee(GameObject go)
    {
        go.SetActive(true);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void triggerNextFromSwitch()
    {
        switch (status)
        {
            case 0: 
                status++;
                Debug.Log("stage 1 Start walking for calibration");
                disableall();
                enablee(status1);
                break;
            case 2:
                status++;
                Debug.Log("stage 3 Assessment init");
                disableall();
                enablee(status3);
                break;
            case 3:
                status++;
                Debug.Log("stage 4 Start walking for assessment");
                disableall();
                enablee(status4);
                break;
            case 5:
                status = 0;
                Debug.Log("stage 0 Init assessment");
                disableall();
                enablee(status0);
                break;
            default:
                break;
        }
    }

    internal void triggerCalibCollision()
    {
        if (status != 1)
            return;
        status++;
        Debug.Log("stage 2 Calib completed");
        disableall();
        enablee(status2);
    }

    internal void triggerAssessCollision()
    {
        if (status != 4)
            return;
        status++;
        Debug.Log("stage 5 assessment completed");
        disableall();
        enablee(status5);
    }
}
