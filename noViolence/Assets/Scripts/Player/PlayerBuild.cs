using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    public GameObject BuildPanel;

    public GameObject mainTentPrefab;
    public GameObject kitchenPrefab;

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
        SpawnPrefab(mainTentPrefab);
    }

    public void SpawnPrefab(GameObject prefab)
    {
        Instantiate(prefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

}
