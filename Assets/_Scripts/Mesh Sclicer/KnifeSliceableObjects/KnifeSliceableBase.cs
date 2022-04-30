using System.Collections;
using BzKovSoft.ObjectSlicer;
using BzKovSoft.ObjectSlicer.Samples;
using UnityEngine;

/// <summary>
/// This script will invoke slice method of IBzSliceableNoRepeat interface if knife slices this GameObject.
/// The script must be attached to a GameObject that have rigidbody on it and
/// IBzSliceable implementation in one of its parent.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class KnifeSliceableBase : MonoBehaviour
{
	IBzSliceableNoRepeat _sliceableAsync;
    AudioSource _audioSource;

    [SerializeField]
    private AudioClip[] _cutSounds;

	void Start()
	{
		_sliceableAsync = GetComponent<IBzSliceableNoRepeat>();
        _audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerExit(Collider other)
	{
		var knife = other.gameObject.GetComponent<Knife>();
		if (knife == null) {
			return;
		}

		StartCoroutine(Slice(knife));
	}

	private IEnumerator Slice(Knife knife)
	{
		// The call from OnTriggerEnter, so some object positions are wrong.
		// We have to wait for next frame to work with correct values
		yield return null;

		Vector3 point = GetCollisionPoint(knife);
		Vector3 normal = Vector3.Cross(knife.MoveDirection, knife.BladeDirection);
		Plane plane = new Plane(normal, point);

		if (_sliceableAsync != null)
		{
            if (_cutSounds.Length > 0) {
                _audioSource.PlayOneShot(_cutSounds[Random.Range(0, _cutSounds.Length)]);
            }

			_sliceableAsync.Slice(plane, knife.SliceID, CallBack);
		}
	}

	protected virtual void CallBack(BzSliceTryResult result) {}

	private Vector3 GetCollisionPoint(Knife knife)
	{
		Vector3 distToObject = transform.position - knife.Origin;
		Vector3 proj = Vector3.Project(distToObject, knife.BladeDirection);

		Vector3 collisionPoint = knife.Origin + proj;
		return collisionPoint;
	}
}

