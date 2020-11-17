using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class ConnectionMenu : MonoBehaviour
{
    public TMP_InputField ipInput;

    public void OnClickHost() => NetworkManager.singleton.StartHost();

    public void OnClickConnect()
    {
        NetworkManager.singleton.networkAddress = string.IsNullOrEmpty(ipInput.text) ? "localhost" : ipInput.text;
    }
}
