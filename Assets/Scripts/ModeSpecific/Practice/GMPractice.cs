using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GMPractice : MonoBehaviour
{
    public TextMeshProUGUI dummyKillCount;

    public GameObject dummyPrefab;
    public GameObject dummyParent;

    private List<Dummy> sceneDummies = new List<Dummy>();
    private List<Vector3> spawnPoints = new List<Vector3>();
    public float respawnTime;

    private void Start()
    {
        if (sceneDummies == null)
        {
            Debug.LogError("**ERROR**\nDummies have not been set for the practice game mode!!");
            return;
        }

        for (int i = 0; i < sceneDummies.Count; i++)
        {
            spawnPoints.Add(sceneDummies[i].transform.position);
        }

        sceneDummies = new List<Dummy>(dummyParent.GetComponentsInChildren<Dummy>());
    }

    private void Update()
    {
        for (int i = 0; i < sceneDummies.Count; i++)
        {
            if (sceneDummies[i] == null)
            {
                Debug.LogError("**ERROR**\nMissing dummy from list!!");
                return;
            }

            if (!sceneDummies[i].gameObject.activeSelf && !sceneDummies[i].isRespawning)
            {
                StartCoroutine(RespawnDummy(respawnTime, sceneDummies[i]));
            }
        }
    }

    public IEnumerator RespawnDummy(float _respawnTime, Dummy _dummy)
    {
        _dummy.isRespawning = true;

        yield return new WaitForSeconds(_respawnTime);

        float _closestDist = float.MaxValue;
        Vector3 _closestSpawnPoint = _dummy.transform.position;
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (Vector3.Distance(transform.position, spawnPoints[i]) < _closestDist)
            {
                _closestDist = Vector3.Distance(transform.position, spawnPoints[i]);
                _closestSpawnPoint = spawnPoints[i];
            }
        }

        GameObject _dummyGO = Instantiate(dummyPrefab, _closestSpawnPoint, Quaternion.identity);
        _dummyGO.transform.parent = dummyParent.transform;

        sceneDummies.Remove(_dummy);
        sceneDummies.Add(_dummyGO.GetComponent<Dummy>());

        _dummy.isRespawning = false;
    }
}
