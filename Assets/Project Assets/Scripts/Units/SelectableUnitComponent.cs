using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnitComponent : MonoBehaviour
{
    private bool isSelected = false;
    private float radius = .5f;

    public void setIsSelected(bool isSelected) {
        // If no change then don't update
        if (this.isSelected == isSelected) {
            return;
        }

        if (this.isSelected) {
            
            // TODO: Hide gameObject that shows that unit is selected
            gameObject.DestroyCircleDraw();

            this.isSelected = false;

        } else {
            // TODO: Show gameObject that shows that unit is selected
            gameObject.CreateCircleDraw(radius);

            this.isSelected = true;
        }
    }
}