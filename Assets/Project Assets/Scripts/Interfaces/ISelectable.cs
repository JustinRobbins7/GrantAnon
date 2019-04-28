using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
  * Interface used to classify scripts that can be selected using the selection circle
  */
public interface ISelectable {
    /**
     * Sets a boolean of the implementing script. Called to change a variable determining whether or not an object is selected.
     */
    void SetSelected(bool selected);

    /**
     * Returns a bool that determines whether or not the implementor is selected.
     */
    bool IsSelected();
}
