using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnitComponent : MonoBehaviour
{
    private bool isSelected = false;

    public void setIsSelected(bool isSelected) {
        Debug.Log("Is selected " + isSelected);

        // If no change then don't update
        if (this.isSelected == isSelected) {
            return;
        }

        if (this.isSelected) {
            // TODO: Hide gameObject that shows that unit is selected

            this.isSelected = false;
        } else {
            // TODO: Show gameObject that shows that unit is selected

            this.isSelected = true;
        }
    }
}