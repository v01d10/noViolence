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

    public GameObject prefab;
    public GameObject model;

    public bool isMoving;
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
        prefabAnimator = GetComponentInChildren<Animator>();
        prefabRenderer = prefab.GetComponentsInChildren<MeshRenderer>();
        SmokePoof = GetComponentInChildren<VisualEffect>();

        isMoving = true;
    }

    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        Placing();
        Building();
    }

    void Placing()
    {
        if (Physics.Raycast(ray, out hit) && isMoving)
        {
            movePrefab();
            rotatePrefab();
            if (!Physics.CheckBox(transform.position, halfExt, transform.rotation, blockBuildLayer))
            {
                for (int i = 0; i < prefabRenderer.Length; i++)
                {
                    prefabRenderer[i].material = prefabWhiteMat;

                }
                if (Input.GetMouseButtonDown(0))
                {
                    placePrefab();
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
                    sfxManager.instance.PlaySFX(sfxManager.instance.buildComplete);
                    isBuilding = false;
                    transform.parent.gameObject.layer = 10;
                    model.SetActive(true);
                    Destroy(prefab);
                    Destroy(this);
                }
            }
            else
            {
                noBuilderIcon.SetActive(true);
                buildTimerText.gameObject.SetActive(false);
                prefabAnimator.Play("noBuilderPulse");
            }
        }
    }

    void movePrefab()
    {
        buildPos = new Vector3(hit.point.x, 0, hit.point.z);
        transform.position = buildPos;
        print(buildPos);
    }

    void rotatePrefab()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(transform.position, Vector3.up, 60 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(transform.position, -Vector3.up, 60 * Time.deltaTime);
        }
    }

    void placePrefab()
    {
        gameObject.layer = 9;
        CameraShakerHandler.Shake(uiManager.instance.buildShake);
        sfxManager.instance.PlaySFX(sfxManager.instance.buildPlacement);
        Invoke("PlaySmoke", smokePlayDelay);

        transform.position = buildPos;
        prefabAnimator.SetTrigger("Place");

        isBuilding = true;
        isMoving = false;
        PlayerManager.instance.playerBuilding = false;
        buildTimer = buildTime;
        buildBar.gameObject.SetActive(true);
    }

    void PlaySmoke()
    {
        SmokePoof.Play();
    }
}

