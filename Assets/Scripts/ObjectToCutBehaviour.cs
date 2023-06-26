using UnityEngine;
using DG.Tweening;

public class ObjectToCutBehaviour : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _endPoint;
    private Vector3 _startPoint;

    private void Start()
    {
        _startPoint = transform.position;
    }
    public void MoveToEnd()
    {
        var tmpPos = transform.position;
        tmpPos.z -= _speed * Time.deltaTime;
        transform.position = tmpPos;
        ResetPosition();
    }
    public void EnableColliders()
    {
        var collider = GetComponentsInChildren<MeshCollider>();
        foreach (var item in collider)
        {
            item.convex = true;
            item.isTrigger = true;
            item.enabled = true;
        }
    }
    public void ResetPosition()
    {
        if (Mathf.Abs(transform.position.z - _endPoint.position.z) > 0.1f) return;
        transform.position = _startPoint;
        EnableColliders();
    }
}
