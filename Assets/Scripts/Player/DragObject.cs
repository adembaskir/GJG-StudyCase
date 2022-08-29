using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [Header("Dragged Object Variables:")]
    public Transform dragThis;
    public LayerMask layerMask;
    public Vector3 offset;
    private Ray ray;
    private RaycastHit raycastHit;
    [Space]
    [Header("Variables:")]
    [SerializeField] private float force = -25f;
    [SerializeField] private float forceOffset = 0.1f;
    [SerializeField] private MeshGenerator waxMeshCreator;
    [SerializeField] private Animator stickAnim;
    [SerializeField] private GameObject stickWax;
    public static DragObject Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this); return;
        }
        Instance = this;
    }
    private void Start()
    {
        TouchController.Instance.onHoldEvent.AddListener(MoveTouchPosition);

    }
    public void MoveTouchPosition(float pressTime)
    {
        if (CanDragged(TouchController.Instance.touch.position, "Arm"))
        {
            dragThis.position = newPosition;
            if (waxMeshCreator)
            {
                Vector3 point = raycastHit.point;
                point += raycastHit.normal * forceOffset;
                waxMeshCreator.AddForceToNormal(point, force);
            }
        }
    }

    public bool CanDragged(Vector3 touchPos, string tag)
    {
        ray = Camera.main.ScreenPointToRay(touchPos);

        if (Physics.Raycast(ray, out raycastHit, layerMask))
        {
            return raycastHit.collider.tag == tag;
        }
        return false;
    }

    public DragObject SetTarget(Transform target)
    {
        dragThis = target;
        return this;
    }
    public DragObject SetOffset(Vector3 _offset)
    {
        offset = _offset;
        return this;
    }
    public Vector3 newPosition
    {
        get
        {
            return raycastHit.point + offset;
        }
    }
    private void MakeItDisable()
    {
        stickAnim.SetTrigger("Disable");
        StartCoroutine(WaitAnimEnd());
       
    }
    IEnumerator WaitAnimEnd()
    {
        yield return new WaitForSeconds(1f);
        stickWax.SetActive(false);
    }
    public void OnEnable()
    {
        EventManager.firstStageCompleted += MakeItDisable;
    }
    public void OnDisable()
    {
        EventManager.firstStageCompleted -= MakeItDisable;
    }
}