using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    [SerializeField] private KnifeBehaviour _knife;
    [SerializeField] private ObjectToCutBehaviour _objectToCut;
    [SerializeField] private float _speed;
    private Touch _touch = new();
    void Update()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                _knife.StartMoving();
            }            
            if (_touch.phase == TouchPhase.Ended)
            {
                _knife.StopMoving();
            }
        }
        
        if (Input.touchCount == 0 && !_knife.Cutting)
        {
                _objectToCut.MoveToEnd();
                _objectToCut.EnableColliders();
        }
    }
}
