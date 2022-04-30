using System;
using BzKovSoft.ObjectSlicer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class KnifeSliceableButton : KnifeSliceableGravity
{
    public bool isOriginal = true;

    [Serializable]
    /// <summary>
    /// Function definition for a button cut event.
    /// </summary>
    public class ButtonCuttedEvent : UnityEvent {}

    // Event delegates triggered on cut.
    [Space(10)]
    [FormerlySerializedAs("onCut")]
    [SerializeField]
    private ButtonCuttedEvent m_OnCut = new ButtonCuttedEvent();

    public ButtonCuttedEvent onCut
    {
        get { return m_OnCut; }
        set { m_OnCut = value; }
    }

	protected override void CallBack(BzSliceTryResult result)
	{
        base.CallBack(result);
        Debug.Log("from bouton");

        if (!isOriginal) {
            return;
        }

        SetCutedObjectToNonOriginal();

        m_OnCut.Invoke();

        void SetCutedObjectToNonOriginal()
        {
            result.outObjectNeg.GetComponent<KnifeSliceableButton>().isOriginal = false;
            result.outObjectPos.GetComponent<KnifeSliceableButton>().isOriginal = false;
        }
	}
}
