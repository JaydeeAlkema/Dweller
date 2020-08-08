using UnityEngine;

/// <summary>
/// A simple smooth camera follow script that smoothly follows the target.
/// </summary>
public class SmoothCamFollow : MonoBehaviour
{
	#region Private Variables
	[SerializeField] private Transform target = default;
	[SerializeField] private float smoothing = default;
	[SerializeField] private Vector3 offset = new Vector3();

	private Vector3 desiredPos = new Vector3();
	private Vector3 smoothedPos = new Vector3();
	#endregion

	#region Monobehaviour Callbacks
	private void Update()
	{
		if(!target) target = GameManager.Instance.PlayerInstance.transform;
	}

	private void FixedUpdate()
	{
		if(target)
		{
			desiredPos = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
			smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothing * Time.fixedDeltaTime);
			transform.position = smoothedPos;
		}
	}
	#endregion
}
