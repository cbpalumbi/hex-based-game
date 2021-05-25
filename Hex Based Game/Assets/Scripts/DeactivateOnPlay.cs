using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnPlay : MonoBehaviour
{
    public bool shouldDeactivate = true;
    void Start()
    {
        if (shouldDeactivate)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
