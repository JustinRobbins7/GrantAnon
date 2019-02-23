using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDraw : MonoBehaviour {
    float theta_scale = 0.01f;        //Set lower to add more points
    int size; //Total number of points in circle
    float radius;
    LineRenderer lineRenderer;

    public void UpdateCircleDraw() {
        if (lineRenderer != null) {
            Vector3 pos;
            float theta = 0f;
            for (int i = 0; i < size; i++) {
                theta += (2.0f * Mathf.PI * theta_scale);
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);
                x += gameObject.transform.position.x;
                y += gameObject.transform.position.y;
                pos = new Vector3(x, y, 0);
                lineRenderer.SetPosition(i, pos);
            }
        }
    }

    public void SetRadius(float radius) {
        this.radius = radius;
    }

    public void InitializeLineRenderer() {
        float sizeValue = (2.0f * Mathf.PI) / theta_scale;
        size = ((int) sizeValue) + 1;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = size;
    }

    public void DestroyLineRenderer() {
        Destroy(lineRenderer);
    }
}
