using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

    // code borrowed from: 
    // https://forum.unity3d.com/threads/getting-the-bounds-of-the-group-of-objects.70979/
    public static Bounds getRenderBounds(GameObject obj){
        var bounds = new  Bounds(Vector3.zero,Vector3.zero);
        var render = obj.GetComponent<Renderer>();
        return render != null? render.bounds : bounds;
    }

    // this too
    public static Bounds getBounds(GameObject obj){
        Bounds bounds;
        Renderer childRender;
        bounds = getRenderBounds(obj);
        if((int)bounds.extents.x == 0){
            bounds = new Bounds(obj.transform.position,Vector3.zero);
            foreach (Transform child in obj.transform) {
                childRender = child.GetComponent<Renderer>();
                if (childRender) {
                    bounds.Encapsulate(childRender.bounds);
                }else{
                    bounds.Encapsulate(getBounds(child.gameObject));
                }
            }
        }
        return bounds;
    }

    // get the center of a gameobject
    public static Vector3 Center (GameObject obj) {
        return Utils.getBounds(obj).center;
    }

    // because % is a remainder on C#, not a modulus... erck.
    // remainder -> can return negative numbers
    // modulus -> always positive
    // blog: https://blogs.msdn.microsoft.com/ericlippert/2011/12/05/whats-the-difference-remainder-vs-modulus/
    public static int Mod (int n, int m){
        return ((n % m) + m) % m;
    }
}
