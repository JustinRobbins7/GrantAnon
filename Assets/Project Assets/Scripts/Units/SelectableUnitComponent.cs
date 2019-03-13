using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnitComponent : MonoBehaviour
{
    /**
     * Allows owning gameobject to be selected, and calls circle draw methods to
     * indicate that the object is selected.
     */

    public int OwningControllerNum = 0;

    private bool isSelected = false;
    private float radius = .5f;

    public void setIsSelected(bool isSelected) {
        // If no change then don't update
        if (this.isSelected == isSelected) {
            return;
        }

        if (this.isSelected) {
            
            // TODO: Hide gameObject that shows that unit is selected
            gameObject.GetComponent<CircleDraw>().DestroyLineRenderer();

            this.isSelected = false;

        } else {
            Debug.Log("Called");
            // TODO: Show gameObject that shows that unit is selected
            gameObject.GetComponent<CircleDraw>().SetRadius(radius);
            if (!gameObject.GetComponent<CircleDraw>().HasLineRenderer()) {
                gameObject.GetComponent<CircleDraw>().InitializeLineRenderer();
                gameObject.GetComponent<CircleDraw>().UpdateCircleDraw();
            }

            this.isSelected = true;
        }
    }
}