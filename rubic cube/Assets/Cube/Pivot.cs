using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    public Vector3 box = new Vector3();

    private List<Part> parts = new List<Part>();

    internal Vector3 prevAngle;

    internal void Rotate(float v)
    {
        transform.Rotate(Vector3.forward, v);        
    }

    public bool Contains(Part part)
    {
        return parts.Contains(part);
    }

    public void RecognizeParts()
    {
        this.parts = new List<Part>();

        var colliders = Physics.OverlapBox(transform.position, box, transform.rotation).ToList();
        foreach (var collider in colliders)
        {
            var part = collider.gameObject.GetComponent<Part>();
            if (part != null)
            {
                this.parts.Add(part);                
            }
        }
    }

    public void AssembleLayer()
    {
        foreach (var part in parts)
        {
            part.transform.SetParent(this.transform);
        }
        prevAngle = transform.localEulerAngles;
    }

    public void DisarmLayer()
    {
        foreach (var part in parts)
        {
            part.transform.SetParent(this.transform.parent);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up);

        Gizmos.color = Color.white;

        GL.PushMatrix();
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, box);
        GL.PopMatrix();

        

        
    }

}
