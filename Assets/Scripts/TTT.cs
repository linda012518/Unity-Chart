using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class TTT : BaseUI
{


    public override void Start()
    {
        DelayDispatch(1, Framework.AreaCode.UI, UIEvent.Push, null); 
        Dispatch(Framework.AreaCode.UI, UIEvent.Push, null); 
    }

}
