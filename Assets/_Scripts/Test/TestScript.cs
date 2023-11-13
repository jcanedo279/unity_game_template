using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class TestScript : MonoBehaviour
{
    VoidEventChannelListener channelListener;
    StringEventChannelListener stringChannelListener;
    [SerializeField] StringEventChannel stringEventChannel;

    public void Start() {
        channelListener = GetComponent<VoidEventChannelListener>();
        stringChannelListener = GetComponent<StringEventChannelListener>();
        // channelListener.RaiseEvent();
        // stringEventChannel.InvokeUnityEvents("hemlo");
    }

    public void TestHandler() {
        print("Hello World!");
    }
    public void TestHandlerString(string someString) {
        print(someString);
    }
}
