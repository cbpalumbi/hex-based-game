using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedObjClickManager : MonoBehaviour
{
    void Update() 
    {
        Check3DObjectClicked();
    }
    
    void Check3DObjectClicked ()
    {
        if (Input.GetMouseButtonDown(0)) {

            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo)) {
                GameObject go = hitInfo.collider.gameObject;
                if (go.layer == 7) //"Mask" layer
                {
                    if(go.transform.parent.GetComponent<Unit>() != null)
                    {
                        go.transform.parent.GetComponent<Unit>().OnMouseDown();
                    }
                }
            }
        }
    }

}
