using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeController : MonoBehaviour
{
    public float delta = 1.0f;
    public int pointAmount = 5;
    public float speedRotation = 1;

    public int numMess = 1;

    private List<Pivot> pivots = new List<Pivot>();

    private bool selected = false;
    private Pivot bestPivot;
    private bool interactable = true;
    private List<Vector2> points = new List<Vector2>();
    private Vector2 lastAngle = new Vector2();
    private float deltaAngle = 0f;

    private Vector2 pivotY;
    private Vector2 pivotX;

    private int[] fixRotation = { 90, -90};

    private void Awake()
    {
        pivots = this.GetComponentsInChildren<Pivot>().ToList();
    }

    private void Start()
    {
        interactable = false;
        StartCoroutine(Mess(numMess));
    }

    internal void OnPartBeginDrag(Part part, PointerEventData pointerEventData)
    {
        if (!interactable) return;

        foreach (var pivot in pivots)
        {
            pivot.RecognizeParts();
        }

        points.Add(pointerEventData.position);
    }

    internal void OnPartDrag(Part part, PointerEventData pointerEventData)
    {
        if (!interactable) return;

        if(selected) // se mueve
        {            
            var pivotPos = MyMath.ZLess(Camera.main.WorldToScreenPoint(bestPivot.transform.position));
                        
            var angle = MyMath.LinearTrans(pointerEventData.position - pivotPos, new Tuple<Vector2, Vector2>(pivotX, pivotY));

            deltaAngle = Vector2.SignedAngle(lastAngle, angle) > 0 ? 1f : -1f;
            lastAngle = angle;

            bestPivot.Rotate(deltaAngle * (pointerEventData.delta).magnitude * speedRotation * Time.deltaTime);
        }
        else if(points.Count > pointAmount) // calcula el movimientos con los punto ya obtenidos
        {
            List<Pivot> relatedPivot = PivotsContainPart(part);
            float bestDistance = Mathf.Infinity;

            foreach (var pivot in relatedPivot) // por cada pivote
            {                


                var pivotPos = MyMath.ZLess(Camera.main.WorldToScreenPoint(pivot.transform.position)); // calculo su pocision en cordenadas de pantalla
                pivotX = MyMath.ZLess(Camera.main.WorldToScreenPoint(pivot.transform.position + pivot.transform.right)) - pivotPos; // el vector (0,1) del plano en que rota transformado a pantalla
                pivotY = MyMath.ZLess(Camera.main.WorldToScreenPoint(pivot.transform.position + pivot.transform.up)) - pivotPos; // el vector (1,0) del plano en que rota transformado a pantalla
                /*Debug.Log(pivot.name+": "+pivotPos+", Y: "+pivotY+", X: "+pivotX);*/
                List<float> difs = new List<float>();

                for (int i = 0; i < (points.Count-1); i++) // por cada punto obtenido
                {
                    var v1 = points[i]- pivotPos; // calculo su diferencia con el pivote
                    var v2 = points[i + 1]- pivotPos; // calculo la diferencia del siguiente con el pivote

                    var v1Trans = MyMath.LinearTrans(v1, new Tuple<Vector2, Vector2>(pivotX, pivotY));
                    var v2Trans = MyMath.LinearTrans(v2, new Tuple<Vector2, Vector2>(pivotX, pivotY));

                    difs.Add(Mathf.Abs( v1Trans.magnitude - v2Trans.magnitude)); 
                }

                float dif = 0;
                float max = 0;
                for (int i = 0; i < difs.Count; i++)
                {
                    dif += difs[i];
                    if(difs[i] > max)
                    {
                        max = difs[i];
                    }
                }
                dif = dif - max;

                if(dif < bestDistance)
                {
                    bestDistance = dif;
                    bestPivot = pivot;
                }

                lastAngle = MyMath.LinearTrans(pointerEventData.position - pivotPos, new Tuple<Vector2, Vector2>(pivotX, pivotY));
            }
            bestPivot.AssembleLayer();
            selected = true;
            
        }
        else if (Vector2.Distance(points.Last(), pointerEventData.position) > delta) // Calcular puntos para predecir el movimiento
        {
            points.Add(pointerEventData.position);
        }
    }

    internal void OnPartEndDrag(Part part, PointerEventData pointerEventData)
    {
        if (!interactable) return;

        interactable = false;
        FixRotation(bestPivot);
        selected = false;
        bestPivot = null;
        points = new List<Vector2>();
        lastAngle = new Vector2();
    }

    private List<Pivot> PivotsContainPart(Part part)
    {
        List<Pivot> toReturn = new List<Pivot>();
        foreach (var pivot in pivots)
        {
            if (pivot.Contains(part))
                toReturn.Add(pivot);
        }
        return toReturn;
    }

    public void FixRotation(Pivot pivot)
    {
        var dif =  pivot.transform.localEulerAngles - pivot.prevAngle;
        pivot.transform.localEulerAngles = new Vector3(
            Mathf.Round(dif.x / 90f) * 90f,
            Mathf.Round(dif.y / 90f) * 90f,
            Mathf.Round(dif.z / 90f) * 90f) + pivot.prevAngle;
        pivot.DisarmLayer();
        interactable = true;
    }

    public IEnumerator Mess(int n)
    {
        var rand = new System.Random();
        for (int i = 0; i < n; i++)
        {            
            var pivot = pivots[rand.Next(0, pivots.Count - 1)];
            pivot.RecognizeParts();
            pivot.AssembleLayer();
            pivot.Rotate(fixRotation[rand.Next(0, fixRotation.Length - 1)]);
            pivot.DisarmLayer();
            yield return new WaitForSeconds(0.5f);
        }
        interactable = true;
    }
}

public static class MyMath
{
    public static Vector2 LinearTrans(Vector2 v, Tuple<Vector2, Vector2> to)
    {
        //return new Vector2((v.x * to.Item1.x) , (v.y * to.Item1.y)) + new Vector2((v.x * to.Item2.x), (v.y * to.Item2.y));
        return ((v.x * to.Item1) + (v.y * to.Item2));
    }

    public static Vector2 ZLess(Vector3 v3)
    {
        return new Vector2(v3.x,v3.y);
    }
}