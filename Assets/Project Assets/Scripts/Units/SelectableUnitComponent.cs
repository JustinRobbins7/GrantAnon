using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnitComponent : MonoBehaviour
{
    public int OwningControllerNum = 0;

    private bool isSelected = false;
    private float radius = .5f;

    public void SetIsSelected(bool isSelected) {
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

    public bool GetIsSelected() {
        return isSelected;
    }
}