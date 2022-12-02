using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iSaveable
{
    object SaveState();
    void LoadState(object state);
}
