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
    /// Function definition for a button click event.
    /// </summary>
    public class ButtonClickedEvent : UnityEvent {}

    // Event delegates triggered on click.
    [Space(10)]
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

	protected override void CallBack(BzSliceTryResult result)
	{
        base.CallBack(result);
        Debug.Log("from bouton");

        if (!isOriginal) {
            return;
        }

        SetCutedObjectToNonOriginal();

        m_OnClick.Invoke();

        void SetCutedObjectToNonOriginal()
        {
            result.outObjectNeg.GetComponent<KnifeSliceableButton>().isOriginal = false;
            result.outObjectPos.GetComponent<KnifeSliceableButton>().isOriginal = false;
        }
	}
}
