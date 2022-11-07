using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    public GameObject BuildPanel;

    public GameObject mainTentPrefab;

    public int mTentTime;

    public void OpenBuildPanel()
    {
        if(!BuildPanel.activeInHierarchy)
        {
            BuildPanel.SetActive(true);
        }
        else
        {
            BuildPanel.SetActive(false);
        }
    }

    public void BuildMainTent()
    {
        SpawnPrefab(mainTentPrefab, mTentTime);
    }

    public void SpawnPrefab(GameObject prefab, int time)
    {
        OpenBuildPanel();
        Instantiate(prefab, transform);
        PlayerManager.instance.playerBuilding = true;
        prefab.GetComponent<PlayerBuildPrefab>().buildTime = time;
    }


}
