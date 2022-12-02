using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using TMPro;
using FirstGearGames.SmoothCameraShaker;

public class PlayerBuildPrefab : MonoBehaviour
{
    Camera cam;
    public float buildTime;
    public float buildTimer;
    public int maxBuilders;

    float buildTimerPerc;

    public GameObject model;

    public bool isMoving;
    public bool isRotating;
    public bool isBuilding;

    public LayerMask blockBuildLayer;

    Ray ray;
    RaycastHit hit;
    Vector3 buildPos;

    public List<PlayerUnit> buildingUnits;

    [Header("Other")]
    Renderer[] prefabRenderer;
    public Material prefabWhiteMat;
    public Material prefabRedMat;

    public Vector3 halfExt = new Vector3(3, 3, 3);

    Animator prefabAnimator;
    VisualEffect SmokePoof;
    public float smokePlayDelay;

    [Header("BuildIndicator")]
    public GameObject buildBarCanvas;
    public Image buildBar;
    public GameObject noBuilderIcon;
    public TextMeshProUGUI buildTimerText;

    void Awake()
    {
        cam = Camera.main;
        prefabAnimator = GetComponent<Animator>();
        prefabRenderer = GetComponentsInChildren<MeshRenderer>();
        SmokePoof = GetComponentInChildren<VisualEffect>();

        isMoving = true;
    }

    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (isMoving)
        {
            Placing();
        }
        else if (isRotating)
        {
            rotatePrefab();

            if (Input.GetMouseButtonDown(0))
            {
                placePrefab();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isMoving = true;
                isRotating = false;
            }
        }
        else if (isBuilding)
        {
            Building();

        }
    }

    void Placing()
    {
        if (Physics.Raycast(ray, out hit) && isMoving)
        {
            movePrefab();

            if (!Physics.CheckBox(transform.position, halfExt, transform.rotation, blockBuildLayer))
            {
                for (int i = 0; i < prefabRenderer.Length; i++)
                {
                    prefabRenderer[i].material = prefabWhiteMat;

                }
                if (Input.GetMouseButtonDown(0))
                {
                    isMoving = false;
                    isRotating = true;
                    isBuilding = false;
                    sfxManager.instance.PlayBuildRotateSound();
                }
            }
            else
            {
                for (int i = 0; i < prefabRenderer.Length; i++)
                {
                    prefabRenderer[i].material = prefabRedMat;

                }
            }
        }
    }

    void Building()
    {
        if (isBuilding)
        {
            if (buildingUnits.Count > 0)
            {
                noBuilderIcon.SetActive(false);
                buildTimerText.gameObject.SetActive(true);
                float dist;

                foreach (PlayerUnit unit in buildingUnits)
                {
                    dist = Vector3.Distance(unit.transform.position, transform.position);

                    if (dist < 1f)
                        buildTimer -= (Time.deltaTime / maxBuilders);
                    else
                        return;
                }


                buildTimerPerc = buildTimer / buildTime;
                buildBar.fillAmount = buildTimerPerc;
                buildTimerText.text = Mathf.Round(buildTimer).ToString();

                if (buildTimer <= 0)
                {
                    LeanAudio.play(sfxManager.instance.buildComplete);
                    Notifier.instance.Notify("Building completed!");
                    isBuilding = false;
                    transform.parent.transform.parent.gameObject.layer = 10;
                    transform.parent.transform.parent.position = transform.position;
                    model.SetActive(true);
                    Destroy(gameObject);
                }
            }
            else
            {
                noBuilderIcon.SetActive(true);
                buildTimerText.gameObject.SetActive(false);
            }
        }
    }

    void movePrefab()
    {
        if (hit.transform.gameObject.layer != 9)
        {
            buildPos = hit.point;
            transform.parent.transform.parent.position = buildPos;
            transform.parent.transform.parent.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            print(buildPos);

        }
    }

    void rotatePrefab()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.RotateAround(transform.parent.transform.parent.position, Vector3.up, 45);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.RotateAround(transform.parent.transform.position, -Vector3.up, 45);
        }
    }

    void placePrefab()
    {
        transform.parent.transform.parent.gameObject.layer = 9;
        PlayerBuildingHolder.instance.PlayerBuildings.Add(transform.parent.transform.parent.GetComponent<Building>());
        CameraShakerHandler.Shake(uiManager.instance.buildShake);
        LeanAudio.play(sfxManager.instance.buildPlacement);
        Invoke("PlaySmoke", smokePlayDelay);

        transform.position = buildPos;

        isMoving = false;
        isRotating = false;
        isBuilding = true;
        PlayerBuildManager.instance.playerBuilding = false;
        buildTimer = buildTime;
        buildBarCanvas.SetActive(true);
    }

    void PlaySmoke()
    {
        SmokePoof.Play();
    }
}

