using System.Collections;
using UnityEngine;

/// <summary>
/// Ejes de coordenadas para el trabajo con el mouse
/// </summary>
public enum MouseAxis {
    x,
    y
}

/// <summary>
/// Rastrea la posición del mouse en tiempo real
/// </summary>

public static class MouseAxisGetter {
#if UNITY_WEBGL
    private static Vector2 lastMousePosition = Vector2.zero;
#endif

    public static float GetMouseAxis (MouseAxis axis) {
#if UNITY_WEBGL
        float axisValue = 0f;

        if (axis == MouseAxis.x) {
            axisValue = (Input.mousePosition.x - lastMousePosition.x) / Screen.width / Time.deltaTime;
        } else if (axis == MouseAxis.y) {
            axisValue = (Input.mousePosition.y - lastMousePosition.y) / Screen.height / Time.deltaTime;
        }

        lastMousePosition = Input.mousePosition;

        return axisValue;
#else
        float axisValue = 0f;
        if (axis == MouseAxis.x) {
            axisValue = Input.GetAxis ("Mouse X");
        } else if (axis == MouseAxis.y) {
            axisValue = Input.GetAxis ("Mouse Y");
        }

        return axisValue;
#endif
    }
}