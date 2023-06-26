using Deform;
using UnityEngine;
/// <summary>
/// Mesh deforming/rolling logic
/// </summary>
public class SlicedMeshBehaviour : MonoBehaviour
{
    private KnifeBehaviour _knife;
    private Collider _knifeColl;
    private Deformable _deformable;
    private TwirlDeformer _deformer;

    private bool _inactive = false;
    private float _minY;
    private float _offsetY = 0.1f;
    private float _zMovement = 0.3f;
    private float _twirlPower = 0f;


    private void Start()
    {
        _deformable = GetComponent<Deformable>();
        _deformer = (TwirlDeformer)_deformable.DeformerElements[0].Component;
    }
    public void InitRoller(KnifeBehaviour knife)
    {
        _knife = knife;
        _knifeColl = _knife.Collider;
        var coll = GetComponent<MeshCollider>();
        coll.convex = true;
        coll.enabled = true;
        _minY = coll.bounds.min.y;
        _twirlPower = 100f / (1 - coll.bounds.min.z);
        coll.enabled = false;

    }
    private void ProgressDeform()
    {
        if (_inactive) return;
        _deformer.Angle += _twirlPower * Time.deltaTime;
        var tmpRot = transform.rotation;
        var tmpPos = transform.position;
        var tmpVector = tmpRot.eulerAngles;
        tmpVector.x += -_twirlPower / 3f * Time.deltaTime;
        tmpPos.z += -_zMovement * Time.deltaTime;
        tmpRot.eulerAngles = tmpVector;
        transform.rotation = tmpRot;
        transform.position = tmpPos;
        if (_knife.AtEnd)
        {
            gameObject.transform.parent = null;
            Destroy(gameObject, 5f);
            gameObject.AddComponent<Rigidbody>();
            _inactive = true;
        }
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            ProgressDeform();
        }
    }
}
