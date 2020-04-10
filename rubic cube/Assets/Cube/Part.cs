using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class Part : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{ 
    public CubeController cube;

    public void OnBeginDrag(PointerEventData eventData)
    {
        cube.OnPartBeginDrag(this,eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        cube.OnPartDrag(this,eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cube.OnPartEndDrag(this,eventData);
    }

    internal void SetCube(CubeController cube)
    {
        this.cube = cube;        
    }

 
}