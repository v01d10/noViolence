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
        if (Input.GetKeyUp(KeyCode.Mouse1) && SelectionManager.instance.SelectedUnits.Count > 0)
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit fHit, FloorLayers))
            {
                int FloorLayer = 7;
                int ResLayer = 8;
                int BuildPrefLayer = 9;
                int BuildingLayer = 10;

                if (fHit.collider.gameObject.layer == FloorLayer)
                {
                    print("Clicked " + fHit.collider.gameObject.layer);
                    foreach (SelectableUnit unit in SelectionManager.instance.SelectedUnits)
                    {
                        if (unit.unitGathering.resTarget != null && unit.unitGathering.resTarget.Gatherers.Contains(unit.unitGathering))
                        {
                            unit.unitGathering.resTarget.Gatherers.Remove(unit.unitGathering);
                            unit.unitGathering.resTarget.placIndex--;
                            unit.unitGathering.toGather = false;
                            unit.unitGathering.toUnload = false;
                            unit.unitGathering.resTarget = null;
                            unit.unitGathering.StopAllCoroutines();
                        }

                        unit.MoveTo(fHit.point);
                    }
                }

                else if (fHit.collider.gameObject.layer == ResLayer)
                {
                    selUnits = SelectionManager.instance.SelectedUnits.ToList();
                    Target = fHit.transform;
                    print("Clicked " + fHit.collider.tag);
                    int i = 0;

                    foreach (SelectableUnit unit in SelectionManager.instance.SelectedUnits)
                    {
                        if (i < fHit.collider.GetComponent<Resource>().MaxGatherers && fHit.collider.GetComponent<Resource>().Gatherers.Count < 3)
                        {
                            unit.unitGathering.StopAllCoroutines();
                            unit.unitGathering.resTarget = fHit.collider.GetComponent<Resource>();
                            unit.unitGathering.resTarget.Gatherers.Add(unit.unitGathering);

                            unit.unitGathering.target = new Vector3(
                                Target.position.x + RadiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / selUnits.Count),
                                Target.position.y,
                                Target.position.z + RadiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / selUnits.Count));
                            unit.MoveTo(Target.position);

                            unit.unitGathering.toUnload = false;
                            i++;
                        }
                        else
                            return;
                    }
                }

                else if (fHit.collider.gameObject.layer == BuildPrefLayer)
                {
                    selUnits = SelectionManager.instance.SelectedUnits.ToList();
                    Target = fHit.transform;
                    print("Clicked " + fHit.collider.tag);
                    PlayerBuildPrefab selBuildPrefab = fHit.collider.GetComponent<PlayerBuildPrefab>();
                    int i = 0;

                    foreach (SelectableUnit unit in SelectionManager.instance.SelectedUnits)
                    {
                        if (unit.unitGathering.resTarget != null && unit.unitGathering.resTarget.Gatherers.Contains(unit.unitGathering))
                        {
                            unit.unitGathering.resTarget.Gatherers.Remove(unit.unitGathering);
                            unit.unitGathering.resTarget.placIndex--;
                            unit.unitGathering.toGather = false;
                            unit.unitGathering.toUnload = false;
                            unit.unitGathering.resTarget = null;
                            unit.unitGathering.StopAllCoroutines();
                        }

                        if (i < 3 && selBuildPrefab.buildingUnits.Count < 3)
                        {
                            PlayerUnit selUnit = unit.GetComponent<PlayerUnit>();
                            selBuildPrefab.buildingUnits.Add(selUnit);
                            unit.MoveTo(Target.position);
                            i++;
                        }

                    }
                }

                else if (fHit.collider.gameObject.layer == BuildingLayer)
                {
                    if (fHit.collider.gameObject.CompareTag("Kitchen"))
                    {
                        Kitchen selKitchen = fHit.collider.GetComponent<Kitchen>();

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
            && hit.collider.TryGetComponent<SelectableUnit>(out SelectableUnit unit))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectionManager.instance.IsSelected(unit))
                    {
                        SelectionManager.instance.Deselect(unit);
                    }
                    else
                    {
                        SelectionManager.instance.Select(unit);
                    }
                }
                else
                {
                    SelectionManager.instance.DeselectAll();
                    SelectionManager.instance.Select(unit);
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
}
