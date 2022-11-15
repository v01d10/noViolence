using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    void Awake() { instance = this; }

    public GameObject UnitPrefab;
    public Transform UnitHolder;

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private RectTransform SelectionBox;
    [SerializeField]
    private LayerMask UnitLayers;
    [SerializeField]
    private LayerMask FloorLayers;
    [SerializeField]
    private LayerMask ResourceLayers;
    [SerializeField]
    private float DragDelay = 0.1f;

    private float MouseDownTime;
    private Vector2 StartMousePosition;

    SelectableUnit sUnit;
[Header("Layers")]
    public int FloorLayer = 7;
    public int ResLayer = 8;
    public int BuildPrefLayer = 9;
    public int BuildingLayer = 10;
    public int PlayerUnitLayer = 11;
    public int EnemyUnitLayer = 12;
    PlayerBuildingDetection playerBdetection;

    [Header("Surround")]
    List<SelectableUnit> selUnits;
    public Transform Target;
    public float RadiusAroundTarget = 0.5f;

    private void Update()
    {
        HandleSelectionInput();
        HandleMovementInputs();

        if (Input.GetKeyDown(KeyCode.H))
        {
            Instantiate(UnitPrefab, UnitHolder);
        }
    }

    private void HandleMovementInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && SelectionManager.instance.SelectedUnits.Count > 0)
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit fHit, FloorLayers))
            {
                if (fHit.collider.gameObject.layer == FloorLayer)
                {
                    selUnits = SelectionManager.instance.SelectedUnits.ToList();
                    print("Clicked " + fHit.collider.gameObject.layer);

                    foreach (SelectableUnit unit in selUnits)
                    {
                        sUnit = unit;
                        sUnit.Agent.isStopped = false;

                        if (sUnit.unitGathering.resTarget != null && sUnit.unitGathering.resTarget.Gatherers.Contains(sUnit.unitGathering))
                        {
                            CancelGathering();
                        }

                        sUnit.MoveTo(fHit.point);
                    }
                }

                else if (fHit.collider.gameObject.layer == ResLayer)
                {
                    selUnits = SelectionManager.instance.SelectedUnits.ToList();
                    Target = fHit.transform;
                    print("Clicked " + fHit.collider.tag);
                    int i = 0;

                    foreach (SelectableUnit unit in selUnits)
                    {
                        sUnit = unit;
                        sUnit.Agent.isStopped = false;

                        if (i < fHit.collider.GetComponent<Resource>().MaxGatherers && fHit.collider.GetComponent<Resource>().Gatherers.Count < 3)
                        {
                            sUnit.unitGathering.StopAllCoroutines();
                            sUnit.unitGathering.resTarget = fHit.collider.GetComponent<Resource>();
                            sUnit.unitGathering.resTarget.Gatherers.Add(sUnit.unitGathering);

                            sUnit.unitGathering.targetPosition = new Vector3(
                                Target.position.x + sUnit.unitGathering.RadiusAroundResource * Mathf.Cos(2 * Mathf.PI * sUnit.unitGathering.resTarget.Gatherers.IndexOf(sUnit.unitGathering) / sUnit.unitGathering.resTarget.Gatherers.Count),
                                Target.position.y,
                                Target.position.z + sUnit.unitGathering.RadiusAroundResource * Mathf.Sin(2 * Mathf.PI * sUnit.unitGathering.resTarget.Gatherers.IndexOf(sUnit.unitGathering) / sUnit.unitGathering.resTarget.Gatherers.Count));
                            sUnit.MoveTo(Target.position);

                            sUnit.unitGathering.toUnload = false;
                            i++;
                        }
                        else
                            return;
                    }
                }

                else if (fHit.collider.gameObject.layer == BuildPrefLayer)
                {
                    PlayerBuildPrefab selBuildPrefab = fHit.collider.GetComponent<PlayerBuildPrefab>();
                    Vector3 placementPosition = selBuildPrefab.transform.position;
                    selUnits = SelectionManager.instance.SelectedUnits.ToList();
                    Target = fHit.transform;
                    print("Clicked " + fHit.collider.tag);
                    int i = 0;

                    foreach (SelectableUnit unit in selUnits)
                    {
                        sUnit = unit;
                        sUnit.Agent.isStopped = false;

                        if (sUnit.unitGathering.resTarget != null && sUnit.unitGathering.resTarget.Gatherers.Contains(sUnit.unitGathering))
                        {
                            CancelGathering();
                        }

                        if (i < 3 && selBuildPrefab.buildingUnits.Count < 3 && !selBuildPrefab.buildingUnits.Contains(sUnit.GetComponent<PlayerUnit>()))
                        {
                            selBuildPrefab.buildingUnits.Add(sUnit.GetComponent<PlayerUnit>());
                            Target.position = new Vector3(
                                Target.position.x + RadiusAroundTarget * 2 * Mathf.Cos(2 * Mathf.PI * i / selUnits.Count),
                                Target.position.y,
                                Target.position.z + RadiusAroundTarget * 2 * Mathf.Sin(2 * Mathf.PI * i / selUnits.Count));
                            selBuildPrefab.transform.position = placementPosition;
                            sUnit.MoveTo(Target.position);
                            i++;
                        }
                    }
                }

                else if (fHit.collider.gameObject.layer == BuildingLayer)
                {
                    if (fHit.collider.gameObject.CompareTag("Kitchen"))
                    {
                        selUnits = SelectionManager.instance.SelectedUnits.ToList();
                        Target = fHit.transform;
                        print("Clicked " + fHit.collider.tag);
                        Kitchen selKitchen = fHit.collider.GetComponent<Kitchen>();

                        foreach (SelectableUnit unit in selUnits)
                        {
                            sUnit = unit;
                            sUnit.Agent.isStopped = false;

                            if (sUnit.unitGathering.resTarget != null && sUnit.unitGathering.resTarget.Gatherers.Contains(sUnit.unitGathering))
                            {
                                CancelGathering();
                            }

                            if (selKitchen.workingUnits.Count < selKitchen.maxWorkingUnits && !selKitchen.workingUnits.Contains(sUnit.GetComponent<PlayerUnit>()))
                            {
                                playerBdetection = sUnit.GetComponentInChildren<PlayerBuildingDetection>();
                                playerBdetection._selKitchen = selKitchen;
                                selKitchen.workingUnits.Add(sUnit.GetComponent<PlayerUnit>());
                                sUnit.MoveTo(Target.position);
                            }
                        }
                    }
                }

                else if (fHit.collider.gameObject.layer == EnemyUnitLayer)
                {
                    selUnits = SelectionManager.instance.SelectedUnits.ToList();
                    Target = fHit.transform;
                    print("Clicked " + fHit.collider.tag);
                    EnemyUnit eUnit = fHit.collider.GetComponent<EnemyUnit>();

                    foreach (SelectableUnit unit in selUnits)
                    {
                        sUnit = unit;
                        sUnit.Agent.isStopped = false;

                        PlayerUnit pUnit = sUnit.GetComponent<PlayerUnit>();

                        if (sUnit.unitGathering.resTarget != null && sUnit.unitGathering.resTarget.Gatherers.Contains(sUnit.unitGathering))
                        {
                            CancelGathering();
                        }

                        CombatDetection combatDetection = sUnit.GetComponentInChildren<CombatDetection>();
                        combatDetection.pUnitTarget = eUnit;
                        sUnit.MoveTo(Target.position);
                    }
                }
            }
        }
    }

    private void HandleSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(true);
            StartMousePosition = Input.mousePosition;
            MouseDownTime = Time.time;
        }
        else if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftShift) && MouseDownTime + DragDelay < Time.time)
        {
            ResizeSelectionBox();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(false);

            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, UnitLayers)
            && hit.collider.TryGetComponent<SelectableUnit>(out SelectableUnit sUnit))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectionManager.instance.IsSelected(sUnit))
                    {
                        SelectionManager.instance.Deselect(sUnit);
                    }
                    else
                    {
                        SelectionManager.instance.Select(sUnit);
                    }
                }
                else
                {
                    SelectionManager.instance.DeselectAll();
                    SelectionManager.instance.Select(sUnit);
                }
            }
            else if (MouseDownTime + DragDelay > Time.time)
            {
                SelectionManager.instance.DeselectAll();
            }

            MouseDownTime = 0;
        }
    }

    private void ResizeSelectionBox()
    {
        float width = Input.mousePosition.x - StartMousePosition.x;
        float height = Input.mousePosition.y - StartMousePosition.y;

        SelectionBox.anchoredPosition = StartMousePosition + new Vector2(width / 2, height / 2);
        SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(SelectionBox.anchoredPosition, SelectionBox.sizeDelta);

        for (int i = 0; i < SelectionManager.instance.AvailableUnits.Count; i++)
        {
            if (UnitIsInSelectionBox(cam.WorldToScreenPoint(SelectionManager.instance.AvailableUnits[i].transform.position), bounds))
            {
                SelectionManager.instance.Select(SelectionManager.instance.AvailableUnits[i]);
            }
            else
            {
                SelectionManager.instance.Deselect(SelectionManager.instance.AvailableUnits[i]);
            }
        }
    }

    private bool UnitIsInSelectionBox(Vector2 Position, Bounds Bounds)
    {
        return Position.x > Bounds.min.x && Position.x < Bounds.max.x && Position.y > Bounds.min.y && Position.y < Bounds.max.y;
    }

    void CancelGathering()
    {
        sUnit.unitGathering.resTarget.Gatherers.Remove(sUnit.unitGathering);
        sUnit.unitGathering.resTarget.placIndex--;
        sUnit.unitGathering.toGather = false;
        sUnit.unitGathering.toUnload = false;
        sUnit.unitGathering.resTarget = null;
        sUnit.unitGathering.gatheringSource.Stop();
        sUnit.unitGathering.StopAllCoroutines();
    }

}
