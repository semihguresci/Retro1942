using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor {

	private void OnSceneGUI () {
		Line line = target as Line;
		Transform handleTransform = line.transform;
		Quaternion handleRotation = handleTransform.rotation ;
		//Vector3 p0 = handleTransform.TransformPoint(line.p0);
		//Vector3 p1 = handleTransform.TransformPoint(line.p1);

		Handles.color = Color.white;
        Vector3 pdirty = Vector3.zero;
        Vector3 prevpdirty = Vector3.zero;
        for ( int i=0;i<line.Points.Count;i++)
        {
            pdirty = handleTransform.TransformPoint(line.Points[i]);
            EditorGUI.BeginChangeCheck();
            pdirty = Handles.DoPositionHandle(pdirty, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(line, "Move Point");
                EditorUtility.SetDirty(line);
                line.Points[i] = handleTransform.InverseTransformPoint(pdirty);
            }

            if(i>0)
            Handles.DrawLine(prevpdirty, pdirty);
            prevpdirty = pdirty;
        }
	
	}
}