using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrab : MonoBehaviour
{
    private Rigidbody2D selectedObject;
    private Vector2 offset;

    [SerializeField] float followStrength;
    [SerializeField] float interpTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D col = Physics2D.OverlapPoint(mousePos);
        
        if (Input.GetMouseButtonDown(0) && col) 
        {
            if(col.transform.gameObject.tag == "grabbable")
            {
                selectedObject = col.attachedRigidbody;
                Vector2 absOffset = (Vector2)col.transform.position - mousePos;
                float angle = -selectedObject.transform.eulerAngles.z * Mathf.PI / 180;
                offset = new Vector2(absOffset.x * Mathf.Cos(angle) + absOffset.y * Mathf.Sin(angle), absOffset.y * Mathf.Cos(angle) + absOffset.x * Mathf.Sin(angle));
                selectedObject.gravityScale = 0;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.gravityScale = 1;
            selectedObject = null;
        }

        if (selectedObject && Input.GetMouseButton(0)) 
        {
            float angle = selectedObject.transform.eulerAngles.z * Mathf.PI / 180;
            Vector2 absOffset = new Vector2(offset.x * Mathf.Cos(angle) + offset.y * Mathf.Sin(angle), offset.y * Mathf.Cos(angle) + offset.x * Mathf.Sin(angle));
            Vector2 targetPoint = (Vector2)selectedObject.transform.position + absOffset;
            Vector2 interpolatedPos = (Vector2)selectedObject.transform.position + selectedObject.velocity*interpTime - absOffset;
            Vector2 movementVector = mousePos - interpolatedPos;
            selectedObject.AddForceAtPosition(movementVector * followStrength, targetPoint);
            //selectedObject.transform.position = mousePos + offset;
            //if (selectedObject.GetComponent<Rigidbody2D>() != null)
            //{
            //    selectedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            //}
        }
        

    }

}
