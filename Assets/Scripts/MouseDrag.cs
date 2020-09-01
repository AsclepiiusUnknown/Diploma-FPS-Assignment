using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    public float forceAmount = 500;
    private Vector3 originalRigidbodyPos;
    private Vector3 originalScreenTargetPosition;
    private Rigidbody selectedRigidbody;
    private float selectionDistance;

    private Camera targetCamera;

    // Start is called before the first frame update
    private void Start()
    {
        targetCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!targetCamera)
            return;
        if (Input.GetMouseButtonDown(0))
            //Check if we are hovering over Rigidbody, if so, select it
            selectedRigidbody = GetRigidbodyFromMouseClick();

        if (Input.GetMouseButtonUp(0) && selectedRigidbody)
            //Release selected Rigidbody if there any
            selectedRigidbody = null;
    }

    private void FixedUpdate()
    {
        if (selectedRigidbody)
        {
            var mousePositionOffset =
                targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    selectionDistance)) - originalScreenTargetPosition;
            selectedRigidbody.velocity =
                (originalRigidbodyPos + mousePositionOffset - selectedRigidbody.transform.position) * forceAmount *
                Time.deltaTime;
        }

        GetRigidbodyFromMouseClick();
    }

    private Rigidbody GetRigidbodyFromMouseClick()
    {
        var hitInfo = new RaycastHit();
        var ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.Raycast(ray, out hitInfo);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.blue);
        if (hit)
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
            {
                selectionDistance = Vector3.Distance(ray.origin, hitInfo.point);
                originalScreenTargetPosition = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, selectionDistance));
                originalRigidbodyPos = hitInfo.collider.transform.position;
                return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            }

        return null;
    }
}