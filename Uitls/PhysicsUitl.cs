using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PhysicsUitl  {



    public static LayerMask LayerMaskWithOut(string _without)
    {
        LayerMask layerWithoutPlayer = new LayerMask();



        layerWithoutPlayer = ~(1 << LayerMask.NameToLayer(_without));
        return layerWithoutPlayer;
    }

    public static LayerMask LayerMaskWithOutPlayer()
    {
        return LayerMaskWithOut("Player");
    }
}
