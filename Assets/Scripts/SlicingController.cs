using Deform;
using EzySlice;
using UnityEngine;
/// <summary>
/// Slice and new mesh creating logic
/// </summary>
public class SlicingController : MonoBehaviour
{     
    [SerializeField] private float _slicesLeftCount = 5f;
    [Header("References")]
    [SerializeField] private KnifeBehaviour _knife;
    [SerializeField] private Transform _slicePlane;
    [SerializeField] private Transform _sliceParentTransform;
    [SerializeField] private TwirlDeformer _deformAxis;
    [SerializeField] private Material _mat;
    [SerializeField] private GameObject _gameEndPanel;


    public void InitializeDeform(GameObject sliced)
    {
        var deformable = sliced.AddComponent<Deformable>();
        var twirl = Instantiate(_deformAxis, sliced.transform);
        twirl.transform.position = new Vector3(0, 1f, -0.5f);
        deformable.AddDeformer(twirl);
        var roller = sliced.AddComponent<SlicedMeshBehaviour>(); 
        var coll = sliced.AddComponent<MeshCollider>();
        coll.enabled = false;
        roller.InitRoller(_knife);

    }
    public void Slice(GameObject target)
    {
        SlicedHull hull = target.Slice(_slicePlane.position, _slicePlane.up);

        if (hull != null)
        {
            var slicedPart = hull.CreateUpperHull(target, _mat);
            slicedPart.name = "SlicedPart";
            var residualPart = hull.CreateLowerHull(target, _mat);
            residualPart.name = "ResidualPart";
            InitializeDeform(slicedPart);
            slicedPart.transform.parent = _sliceParentTransform;
            residualPart.transform.parent = _sliceParentTransform;
            slicedPart.transform.position = target.transform.position;
            residualPart.transform.position = target.transform.position;
           var coll = residualPart.AddComponent<MeshCollider>();
            coll.convex = true;
            coll.isTrigger = true;
            coll.enabled = false;
            target.gameObject.SetActive(false);
            Destroy(target, 0.1f);

        }
    }

    public void SliceCycleDone()
    {
        _slicesLeftCount--;
        if (_slicesLeftCount == 0)
        {
            _gameEndPanel.SetActive(true);
            Time.timeScale = 0;
        }

    }
}
