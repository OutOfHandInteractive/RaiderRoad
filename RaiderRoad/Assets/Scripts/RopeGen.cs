using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RopeGen : EditorWindow
{
    Transform parent = null;
    Transform ropeSegment = null;
    private float positionOffset = -0.075f;
    int numberOfSegments = 1;
    
    [MenuItem("Window/RopeGen")]
    public static void ShowWindow()
    {
        GetWindow<RopeGen>("Rope Generation");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        parent = EditorGUILayout.ObjectField("Parent Object", parent, typeof(Transform), true) as Transform;
        ropeSegment = EditorGUILayout.ObjectField("Rope Segment Prefab", ropeSegment, typeof(Transform), true) as Transform;
        positionOffset = EditorGUILayout.FloatField("Position Offset", positionOffset);
        numberOfSegments = EditorGUILayout.IntSlider("Number of Segments", numberOfSegments, 1, 100);
        if (GUILayout.Button("Generate"))
        {
            GenerateRope();
        }
    }
 
    private void GenerateRope()
    {
        if (parent != null && ropeSegment != null)
        {
            Transform previousSeg = Instantiate(ropeSegment, parent);
            previousSeg.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            previousSeg.name = "RopeSegment 0";
            for (int i = 1; i < numberOfSegments; i++)
            {
                Transform currentSeg = Instantiate(ropeSegment, parent);
                currentSeg.localPosition = new Vector3(0f, (float)i * positionOffset, 0f);
                previousSeg.gameObject.GetComponent<ConfigurableJoint>().connectedBody = currentSeg.gameObject.GetComponent<Rigidbody>();
                currentSeg.name = "RopeSegment " + i;
                previousSeg = currentSeg;
            }
            Destroy(previousSeg.gameObject.GetComponent<ConfigurableJoint>());
        }
    }
}
