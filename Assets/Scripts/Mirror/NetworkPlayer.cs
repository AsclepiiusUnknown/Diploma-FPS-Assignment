using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public string username = "Player";

    public GameObject lobbyPlayer;
    public GameObject gameplayPlayer;

    public string lobbyScene = "Lobby";
    public string gameScene = "Gameplay";

    private bool connectedToLobbyUI = false;

    private LobbyMenu lobby;

    public override void OnStartLocalPlayer()
    {
        SceneManager.LoadSceneAsync(lobbyScene, LoadSceneMode.Additive);
    }

    public void ReadyPlayer(int _index, bool _isReady)
    {
        if (isLocalPlayer)
            CmdReadyPlayer(_index, _isReady);
    }

    public void StartGame()
    {
        if (isLocalPlayer)
            CmdStartGame();
    }

    public void SetName(string _name)
    {
        if (isLocalPlayer)
            CmdSetPlayerName(_name);
    }

    private void Start()
    {
        lobbyPlayer.SetActive(true);
        gameplayPlayer.SetActive(false);
    }

    private void Update()
    {
        if (lobby == null && lobbyPlayer.activeSelf)
        {
            lobby = FindObjectOfType<LobbyMenu>();
        }

        if (!connectedToLobbyUI && lobby != null)
        {
            lobby.OnPlayerConnect(this);
            connectedToLobbyUI = true;
        }
    }

    [Command] public void CmdReadyPlayer(int _index, bool _isReady) => RpcReadyPlayer(_index, _isReady);
    [ClientRpc] public void RpcReadyPlayer(int _index, bool _isReady) => lobby?.SetReadyPlayer(_index, _isReady);

    [Command] public void CmdSetPlayerName(string _name) => RpcSetPlayerName(_name);
    [ClientRpc] public void RpcSetPlayerName(string _name) => username = _name;

    [Command] public void CmdStartGame() => RpcStartGame();
    [ClientRpc]
    public void RpcStartGame()
    {
        NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();

        foreach (NetworkPlayer player in players)
        {
            player.lobbyPlayer.SetActive(false);
            player.gameplayPlayer.SetActive(true);

            if (player.isLocalPlayer)
            {
                SceneManager.UnloadSceneAsync(lobbyScene);
                StartCoroutine(LoadGameScene());

                gameplayPlayer.GetComponent<FPS.FpsCustomNetworked>().Setup();
            }
        }
    }

    private IEnumerator LoadGameScene()
    {
        yield return SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Additive);

        Scene scene = SceneManager.GetSceneByName(gameScene);
        SceneManager.SetActiveScene(scene);
    }
}
