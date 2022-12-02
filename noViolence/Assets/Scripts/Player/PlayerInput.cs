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
    private float DragDelay = 0.1f;

    private float MouseDownTime;
    private Vector2 StartMousePosition;

    [Header("Layers")]
    public int FloorLayer;
    public int ResourceLayer;
    public int BuildPrefabLayer;
    public int BuildingLayer;
    public int PlayerUnitLayer;
    public int EnemyUnitLayer;

    SelectableUnit sUnit;
    PlayerUnit sPlayerUnit;

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
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit fHit))
            {
                if (fHit.collider.gameObject.layer == 7)
                {
                    selUnits = SelectionManager.instance.SelectedUnits.ToList();
                    print("Clicked " + fHit.collider.gameObject.layer);

                    foreach (SelectableUnit unit in selUnits)
                    {
                        sUnit = unit;
                        sUnit.Agent.isStopped = false;
                        sUnit.thisPunit.CancelAllActions();
                        sUnit.MoveTo(fHit.point);
                    }
                }

                else if (fHit.collider.gameObject.layer == ResourceLayer)
                {
                    selUnits = SelectionManager.instance.SelectedUnits.ToList();
                    Target = fHit.transform;
                    print("Clicked " + fHit.collider.tag);

                    foreach (SelectableUnit unit in selUnits)
                    {
                        sUnit = unit;
                        sUnit.Agent.isStopped = false;
                        sUnit.thisPunit.CancelAllActions();

                        if (fHit.collider.GetComponent<Resource>().Gatherers.Count < fHit.collider.GetComponent<Resource>().MaxGatherers)
                        {
                            sUnit.thisPunit.pUnitGathering.resTarget = fHit.collider.GetComponent<Resource>();
                            sUnit.thisPunit.pUnitGathering.resTarget.Gatherers.Add(sUnit.thisPunit.pUnitGathering);

                            sUnit.thisPunit.pUnitGathering.targetPosition = new Vector3(
                                Target.position.x + sUnit.thisPunit.pUnitGathering.RadiusAroundResource * Mathf.Cos(2 * Mathf.PI * sUnit.thisPunit.pUnitGathering.resTarget.Gatherers.IndexOf(sUnit.thisPunit.pUnitGathering) / sUnit.thisPunit.pUnitGathering.resTarget.Gatherers.Count),
                                Target.position.y,
                                Target.position.z + sUnit.thisPunit.pUnitGathering.RadiusAroundResource * Mathf.Sin(2 * Mathf.PI * sUnit.thisPunit.pUnitGathering.resTarget.Gatherers.IndexOf(sUnit.thisPunit.pUnitGathering) / sUnit.thisPunit.pUnitGathering.resTarget.Gatherers.Count));
                            sUnit.MoveTo(Target.position);
                            sUnit.thisPunit.pUnitGathering.StartGathering();
                        }
                        else
                            return;
                    }
                }

                else if (fHit.collider.gameObject.layer == BuildPrefabLayer)
                {
                    PlayerBuildPrefab selBuildPrefab = fHit.collider.GetComponentInChildren<PlayerBuildPrefab>();
                    Vector3 placementPosition = selBuildPrefab.transform.position;
                    selUnits = SelectionManager.instance.SelectedUnits.ToList();
                    Target = fHit.transform;
                    print("Clicked " + fHit.collider.tag);
                    int i = 0;

                    foreach (SelectableUnit unit in selUnits)
                    {
                        sUnit = unit;
                        sUnit.Agent.isStopped = false;
                        sUnit.thisPunit.CancelAllActions();

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
                    selUnits = SelectionManager.instance.SelectedUnits.ToList();
                    Target = fHit.transform;
                    print("Clicked " + fHit.collider.tag);

                    if (fHit.collider.gameObject.CompareTag("Kitchen"))
                    {
                        Kitchen selKitchen = fHit.collider.GetComponent<Kitchen>();

                        foreach (SelectableUnit unit in selUnits)
                        {
                            sUnit = unit;
                            sUnit.Agent.isStopped = false;
                            sUnit.thisPunit.CancelAllActions();

                            if (selKitchen.workingUnits.Count < selKitchen.maxWorkingUnits && !selKitchen.workingUnits.Contains(sUnit.GetComponent<PlayerUnit>()))
                            {
                                sUnit.thisPunit.playerBdetection._selKitchen = selKitchen;
                                sUnit.thisPunit.playerBdetection.StartBuildingDetection();

                                if (uiManager.instance.KitchenScroll.activeInHierarchy)
                                {
                                    selKitchen.DestroyWorkerPanels();
                                    selKitchen.LoadWorkerPanels();
                                }

                                if (KitchenUI.kitchenUI.ProdScroll.activeInHierarchy)
                                {
                                    selKitchen.LoadKitchenText();
                                }

                                selKitchen.workingUnits.Add(sUnit.GetComponent<PlayerUnit>());
                                sUnit.MoveTo(Target.position);
                            }
                        }
                    }

                    else if (fHit.collider.gameObject.CompareTag("Church"))
                    {
                        Church selChurch = fHit.collider.GetComponent<Church>();

                        foreach (SelectableUnit unit in selUnits)
                        {
                            sUnit = unit;
                            sUnit.Agent.isStopped = false;
                            sUnit.thisPunit.CancelAllActions();

                            if (selChurch.workingUnits.Count < selChurch.maxWorkingUnits && !selChurch.workingUnits.Contains(sUnit.GetComponent<PlayerUnit>()))
                            {
                                sUnit.thisPunit.playerBdetection._selChurch = selChurch;
                                sUnit.thisPunit.playerBdetection.StartBuildingDetection();

                                if (uiManager.instance.KitchenScroll.activeInHierarchy)
                                {
                                    selChurch.DestroyWorkerPanels();
                                    selChurch.LoadWorkerPanels();
                                }

                                if (KitchenUI.kitchenUI.ProdScroll.activeInHierarchy)
                                {
                                    selChurch.LoadChurchText();
                                }

                                selChurch.workingUnits.Add(sUnit.GetComponent<PlayerUnit>());
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
                    EnemyUnit eUnit = fHit.collider.GetComponentInParent<EnemyUnit>();

                    foreach (SelectableUnit unit in selUnits)
                    {
                        sUnit = unit;
                        sUnit.Agent.isStopped = false;
                        sUnit.thisPunit.CancelAllActions();

                        if (!eUnit.pUnitsFighWith.Contains(sUnit.thisPunit))
                        {
                            eUnit.pUnitsFighWith.Add(sUnit.thisPunit);
                        }

                        sUnit.GetComponentInChildren<CombatDetection>().pUnitTarget = eUnit;
                        sUnit.GetComponentInChildren<CombatDetection>().StartCombat();
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

            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, PlayerUnitLayer)
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
            if (SelectionManager.instance.AvailableUnits[i] != null)
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
    }

    private bool UnitIsInSelectionBox(Vector2 Position, Bounds Bounds)
    {
        return Position.x > Bounds.min.x && Position.x < Bounds.max.x && Position.y > Bounds.min.y && Position.y < Bounds.max.y;
    }

}
