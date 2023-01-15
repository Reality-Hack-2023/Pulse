using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sngty;
using System;
using TMPro;

public class CommunicationManager : MonoBehaviour
{
    public SingularityManager singularityManager;
    public StateMachine stateMachine;

    List<DeviceSignature> pairedDevices;

    public TMP_Text messageText;
    // Start is called before the first frame update
    void Start()
    {
        ConnectandGet();
    }

    // Update is called once per frame
    void Update()
    {
        
        //if(Input.GetKeyDown(KeyCode.L))
        if(OVRInput.GetDown(OVRInput.Button.One))
        {
            DebugLog("Getting started");
            //ConnectandGet();
            singularityManager.sendMessage("start-hr", pairedDevices[0]);
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            DebugLog("Getting stopped");
            singularityManager.sendMessage("stop-hr", pairedDevices[0]);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            TriggerState();
        }
    }

    private void DebugLog(string v)
    {
        Debug.Log("MBMB: "+v);
    }

    public void ConnectandGet()
    {
        DebugLog("Connect start");
        pairedDevices = singularityManager.GetPairedDevices(); 
        DebugLog("Connect pair");
        if (pairedDevices.Count==0)
        {
            DebugLog("No devices paired");
            return;
        }

        foreach (var item in pairedDevices)
        {
            DebugLog("Name is " + item.name);
        }

        singularityManager.ConnectToDevice(pairedDevices[0]);

        singularityManager.onMessageRecieved.AddListener(OnCC);

        Debug.Log("Name "+pairedDevices[0].name);

        

    }


    public void onConnected()
    {
        DebugLog("Connected to device!");
    }

    public void OnCC(string message)
    {
        DebugLog("Message recieved from deviceeeeee: " + message);
    }

    public void onMessageRecieved(string message)
    {
        DebugLog("Message recieved from device: " + message);
        Displaymessage(message);

        if(message=="switch")
        {
            TriggerState();
        }
    }

    public void onError(string errorMessage)
    {
        DebugLog("Error with Singularity: " + errorMessage);
    }

    private void Displaymessage(string message)
    {
        messageText.text = (count++) + ": "+message;
    }
    int count = 0;

    public void TriggerState()
    {
        stateMachine.triggerNextFromSwitch();
    }
}
