using BzKovSoft.ObjectSlicer;
using UnityEngine;

public class KnifeSliceableGravity : KnifeSliceableBase
{
	protected override void CallBack(BzSliceTryResult result)
	{
        result.outObjectNeg.GetComponent<Rigidbody>().isKinematic = false;
        result.outObjectPos.GetComponent<Rigidbody>().isKinematic = false;
        Debug.Log("from gravity");
	}
}
