using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable
{
    void SetOwningPlayerNum(int owningPlayerNum);
    int GetOwningPlayerNum();
}
