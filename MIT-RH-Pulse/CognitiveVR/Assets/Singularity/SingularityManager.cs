using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sngty
{
    public class SingularityManager : MonoBehaviour
    {
        public UnityEvent onConnected;
        public UnityEvent<string> onMessageRecieved;
        public UnityEvent<string> onError;

        private AndroidJavaClass BluetoothManager;
        private AndroidJavaObject bluetoothManager;

        private List<AndroidJavaObject> connectedDevices;

        // Start is called before the first frame update
        void Awake()
        {
            BluetoothManager = new AndroidJavaClass("com.harrysoft.androidbluetoothserial.BluetoothManager");
            bluetoothManager = BluetoothManager.CallStatic<AndroidJavaObject>("getInstance");

            connectedDevices = new List<AndroidJavaObject>();
            Debug.Log("Singularity star completed");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ConnectToDevice(DeviceSignature sig)
        {
            AndroidJavaClass Schedulers = new AndroidJavaClass("io.reactivex.schedulers.Schedulers");
            AndroidJavaClass AndroidSchedulers = new AndroidJavaClass("io.reactivex.android.schedulers.AndroidSchedulers");
            bluetoothManager.Call<AndroidJavaObject>("openSerialDevice", sig.mac)
                            .Call<AndroidJavaObject>("subscribeOn",Schedulers.CallStatic<AndroidJavaObject>("io"))
                            .Call<AndroidJavaObject>("observeOn", AndroidSchedulers.CallStatic<AndroidJavaObject>("mainThread"))
                            .Call("subscribe", new RxSingleObserver(onError, onConnected, onMessageRecieved, connectedDevices));

        }

        public void sendMessage(string message, DeviceSignature sig)
        {
            for (int i = 0; i < connectedDevices.Count; i++)
            {
                if (connectedDevices[i].Get<string>("mac") == sig.mac)
                {
                    AndroidJavaObject connectedDevice = connectedDevices[i];
                    AndroidJavaObject deviceInterface = connectedDevice.Call<AndroidJavaObject>("toSimpleDeviceInterface");
                    deviceInterface.Call("sendMessage", message);
                    break;
                }
            }
        }

        public void DisconnectDevice(DeviceSignature sig)
        {
            bluetoothManager.Call("closeDevice", sig.mac);
            for (int i = 0; i < connectedDevices.Count; i++)
            {
                if (connectedDevices[i].Get<string>("mac") == sig.mac)
                {
                    connectedDevices.RemoveAt(i);
                    break;
                }
            }
        }

        public void DisconnectAll()
        {
            bluetoothManager.Call("close");
            connectedDevices.Clear();
        }

        public List<DeviceSignature> GetPairedDevices()
        {
            Debug.Log("Line 1");
            AndroidJavaObject pairedDevicesCollection = bluetoothManager.Call<AndroidJavaObject>("getPairedDevices");
            Debug.Log("Line 2");
            AndroidJavaObject pairedDevicesIterator = pairedDevicesCollection.Call<AndroidJavaObject>("iterator");
            Debug.Log("Line 3");
            int size = pairedDevicesCollection.Call<int>("size");
            Debug.Log("Line 4");
            List<DeviceSignature> pairedDevices = new List<DeviceSignature>();
            Debug.Log("Line 5");
            for (int i = 1; i <= size; i++)
            {
                Debug.Log("Line 6");
                AndroidJavaObject thisDevice = pairedDevicesIterator.Call<AndroidJavaObject>("next");
                Debug.Log("Line 7");
                DeviceSignature thisSignature;
                thisSignature.name = thisDevice.Call<string>("getName");
                thisSignature.mac = thisDevice.Call<string>("getAddress");
                pairedDevices.Add(thisSignature);
            }
            return pairedDevices;
        }

        class RxSingleObserver : AndroidJavaProxy
        {
            private UnityEvent<string> onErrorEvent;
            private UnityEvent onConnectedEvent;
            private UnityEvent<string> onMessageRecievedEvent;
            private List<AndroidJavaObject> connectedDevices;
            public RxSingleObserver(UnityEvent<string> onErrorEvent, UnityEvent onConnectedEvent, UnityEvent<string> onMessageRecievedEvent, List<AndroidJavaObject> connectedDevices) : base("io.reactivex.SingleObserver")
            {
                this.onErrorEvent = onErrorEvent; 
                this.onConnectedEvent = onConnectedEvent;
                this.onMessageRecievedEvent = onMessageRecievedEvent;
                this.connectedDevices = connectedDevices;
            }

            void onError(AndroidJavaObject e) //e is type throwable in Java
            {
                Debug.LogWarning("Singularity BLUETOOTH ERROR");

                onErrorEvent.Invoke("Singularity: " + e.Call<string>("getMessage"));
            }

            void onSuccess(AndroidJavaObject connectedDevice) //connectedDevice is type BluetoothSerialDevice in Java
            {
                onConnectedEvent.Invoke();

                AndroidJavaObject deviceInterface = connectedDevice.Call<AndroidJavaObject>("toSimpleDeviceInterface");
                deviceInterface.Call("setMessageReceivedListener", new messageRecievedListener(onMessageRecievedEvent));
                connectedDevices.Add(connectedDevice);
            }

            void onSubscribe(AndroidJavaObject obj){}
        }

        class messageRecievedListener : AndroidJavaProxy
        {
            private UnityEvent<string> onMessageRecievedEvent;
            public messageRecievedListener(UnityEvent<string> onMessageRecievedEvent) : base("com.harrysoft.androidbluetoothserial.SimpleBluetoothDeviceInterface$OnMessageReceivedListener")
            {
                this.onMessageRecievedEvent = onMessageRecievedEvent;
            }

            void onMessageReceived(string message)
            {
                onMessageRecievedEvent.Invoke(message);
            }
        }

    }
    public struct DeviceSignature
    {
        public string name;
        public string mac;
    }
}