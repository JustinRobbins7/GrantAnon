using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CircleDraw {
    public static void UpdateCircleDraw(this GameObject gameObject, float radius) {
        if (HasCircleDraw(gameObject)) {

            Vector3 pos;
            float theta = 0f;
            for (int i = 0; i < GetCircleDrawNumPoints(gameObject); i++) {
                theta += (2.0f * Mathf.PI * 0.01f);
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);
                x += gameObject.transform.position.x;
                y += gameObject.transform.position.y;
                pos = new Vector3(x, y, 0);
                GetCircleDraw(gameObject).SetPosition(i, pos);

                GetCircleDraw(gameObject).sortingLayerName = "Foreground";
            }
        }
    }

    public static bool HasCircleDraw(this GameObject gameObject) {
        return gameObject.GetComponent<LineRenderer>() != null;
    }

    public static int GetCircleDrawNumPoints(this GameObject gameObject) {
        return gameObject.GetComponent<LineRenderer>().positionCount;
    }

    public static LineRenderer GetCircleDraw(this GameObject gameObject) {
        return HasCircleDraw(gameObject) ? gameObject.GetComponent<LineRenderer>() : null;
    }

    public static void CreateCircleDraw(this GameObject gameObject, float radius, float width = 0.1f) {
        if (!HasCircleDraw(gameObject)) {
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
            lineRenderer.positionCount = ((int)(2.0f * Mathf.PI / 0.01f)) + 1;
            UpdateCircleDraw(gameObject, radius);
        }
    }

    public static void DestroyCircleDraw(this GameObject gameObject) {
        if (HasCircleDraw(gameObject)) {
            Object.Destroy(GetCircleDraw(gameObject));
        }
    }
}
