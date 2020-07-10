using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor {

	private int lineSteps = 10;
	private const float directionScale = 0.5f;

	private BezierCurve curve;
	private Transform handleTransform;
	private Quaternion handleRotation;

	private void OnSceneGUI () {
		curve = target as BezierCurve;
        lineSteps = Mathf.RoundToInt(curve.points.Length*2.5f);
        handleTransform = curve.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ?
			handleTransform.rotation : Quaternion.identity;
		

        for(int i=3;i< curve.points.Length;i++)
        {
            Vector3 p0 = ShowPoint(i-3);
            Vector3 p1 = ShowPoint(i-2);
            Vector3 p2 = ShowPoint(i-1);
            Vector3 p3 = ShowPoint(i);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p3);

            ShowDirections();
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
        }

		
	}

	private void ShowDirections () {
		Handles.color = Color.green;
		Vector3 point = curve.GetPoint(0f);
		Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
		for (int i = 0; i <= lineSteps; i++) {
			point = curve.GetPoint(i / (float)lineSteps);
			Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
		}
	}

	private Vector3 ShowPoint (int index) {
		Vector3 point = handleTransform.TransformPoint(curve.points[index]);
		EditorGUI.BeginChangeCheck();
		point = Handles.DoPositionHandle(point, handleRotation);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(curve, "Move Point");
			EditorUtility.SetDirty(curve);
			curve.points[index] = handleTransform.InverseTransformPoint(point);
		}
		return point;
	}
}