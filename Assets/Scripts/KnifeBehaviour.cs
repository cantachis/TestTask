using UnityEngine;
using DG.Tweening;


/// <summary>
/// Knife movement and Slicing behaviour
/// </summary>
public class KnifeBehaviour : MonoBehaviour
{
    [SerializeField] private SlicingController _sliceController;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _timeForOneSlice = 2f;

    private bool _disabled = false;

    public bool Disabled => _disabled; 
    public bool Cutting { get; set; }
    public bool AtEnd { get => (Mathf.Abs(_endPoint.position.y - transform.position.y) < 0.2f); }
    public bool AtStart { get => (Mathf.Abs(_startPoint.position.y - transform.position.y) < 0.2f); }
    public Collider Collider => GetComponentInChildren<MeshCollider>();

    private void Awake()
    {
        Cutting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Cutting = true;
        if (_disabled) return;
        _sliceController.Slice(other.gameObject);
    }

    public void StartMoving()
    {
        MoveToEnd();
    }
    public void StopMoving()
    {
        if (_disabled) return;
        transform.DOKill();
        if (!Cutting) ResetKnifePosition();
    }
    private void CheckForEnd()
    {
        if (!AtEnd) return;
        _disabled = true;
        ResetKnifePosition();
        
    }
    private void CheckForStart()
    {
        if (!AtStart) return;
        _disabled = false;
        Cutting = false; 
        _sliceController.SliceCycleDone();
        ResetKnifePosition();
    }
    public void MoveToEnd()
    {
        transform.DOKill();
        transform.DOMove(_endPoint.position, _timeForOneSlice / 2).SetEase(Ease.Linear).OnComplete(CheckForEnd);
        transform.DORotate(_endPoint.rotation.eulerAngles, _timeForOneSlice / 2).SetEase(Ease.Linear);

    }
    private void ResetKnifePosition()
    {
        transform.DOKill();
        transform.DOMove(_startPoint.position, _timeForOneSlice / 2).SetEase(Ease.Linear).OnComplete(CheckForStart);
        transform.DORotate(_startPoint.rotation.eulerAngles, _timeForOneSlice / 2).SetEase(Ease.Linear);

    }
}
