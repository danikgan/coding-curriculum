using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{
    public SceneReferences ReferencesScript;

    private GameObject DragArea;
    private GameObject DropArea;

    Vector3 dist;
    float posX;
    float posY;

    //private bool MovingObject = false;

    void Start()
    {
        DragArea = ReferencesScript.DragArea;
        DropArea = ReferencesScript.DropArea;
    }

    void OnMouseDown() //Start moving the object
    {
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;

        if (transform.parent == DragArea.transform)
        {
            Transform thisTransform = Instantiate(transform, transform.position, Quaternion.identity) as Transform;
            thisTransform.parent = DragArea.transform;
        }
    }

    void OnMouseDrag() //Moving the object
    {
        Vector3 curPos =
                  new Vector3(Input.mousePosition.x - posX,
                  Input.mousePosition.y - posY, dist.z);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
    }

    void OnMouseUp() //Droping the object
    {
        Vector3 adjusted2Dposition = transform.position;
        adjusted2Dposition.z = 0;
        if (DropArea.GetComponent<Renderer>().bounds.Contains(adjusted2Dposition))
        {
            transform.parent = DropArea.transform;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
