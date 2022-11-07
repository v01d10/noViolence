using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBuildPrefab : MonoBehaviour
{
    public float buildTime;
    public float buildTimer;

    float buildTimerPerc;

    public GameObject prefab;
    public GameObject model;

    public GameObject buildBarCanvas;
    public Image buildBar;
    public TextMeshProUGUI buildTimerText;

    public bool isMoving;
    public bool isBuilding;

    Ray ray;
    RaycastHit hit;

    Vector3 buildPos;
    public Vector3 halfExt = new Vector3(3, 3, 3);

    Camera cam;
    Animator prefabAnimator;

    void Start()
    {
        cam = Camera.main;
        prefabAnimator = GetComponentInChildren<Animator>();

        isMoving = true;
    }

    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Building();
        }

        if (isBuilding)
        {
            buildTimer -= Time.deltaTime;
            buildBarCanvas.SetActive(true);
            buildTimerPerc = buildTimer / buildTime;
            buildBar.fillAmount = buildTimerPerc;
            buildTimerText.text = Mathf.Round(buildTimer).ToString();


            if (buildTimer <= 0)
            {
                isBuilding = false;
                Destroy(prefab);
                Destroy(this);
                model.SetActive(true);
            }
        }
    }

    void Building()
    {
        if (isMoving && !Physics.CheckBox(hit.point, halfExt, transform.rotation, 7))
        {
            movePrefab();

            rotatePrefab();

            if (Input.GetMouseButtonDown(0))
            {
                placePrefab();
            }
        }
    }

    void movePrefab()
    {
        buildPos = hit.point;
        transform.position = buildPos;
        print(buildPos);
    }

    void rotatePrefab()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(transform.position, Vector3.up, 30 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(transform.position, -Vector3.up, 30 * Time.deltaTime);
        }
    }

    void placePrefab()
    {
        transform.position = buildPos;
        prefabAnimator.SetTrigger("Place");

        isBuilding = true;
        isMoving = false;
        PlayerManager.instance.playerBuilding = false;
        buildTimer = buildTime;
    }
}

