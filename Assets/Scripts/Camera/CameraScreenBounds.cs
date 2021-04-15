using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScreenBounds : MonoBehaviour
{
    [SerializeField] private BoundsVariable screenBounds;
    [SerializeField] private Vector3 screenSizeMultiplier = Vector3.one;
    private Vector3 m_screenBoundsSize;
    private Vector2 m_screenSize;
    private Vector3 m_position;

    private Camera m_camera;

    [SerializeField] private float checkScreenSizeTime = 0.5f;
    private WaitForSeconds m_waitForSeconds;

    private void Start()
    {
        m_camera = GetComponent<Camera>();
        SetScreenBounds();

        if (!(checkScreenSizeTime > 0)) return;

        m_waitForSeconds = new WaitForSeconds(checkScreenSizeTime);
        StartCoroutine(CheckScreenSize());
    }

    private void SetScreenBounds()
    {
        m_position = transform.position;
        m_screenSize = new Vector2(Screen.width, Screen.height);
        m_screenBoundsSize = m_camera.ScreenToWorldPoint(new Vector3(m_screenSize.x * screenSizeMultiplier.x,
                                                                     m_screenSize.y * screenSizeMultiplier.y,
                                                                     m_position.z * screenSizeMultiplier.z));

        screenBounds.SetValue(new Bounds(m_position, m_screenBoundsSize));
    }

    private IEnumerator CheckScreenSize()
    {
        while (checkScreenSizeTime > 0)
        {
            if (Math.Abs(Screen.width - m_screenSize.x) > 0.01f
                || Math.Abs(Screen.height - m_screenSize.y) > 0.01f
                || m_position != transform.position)
            {
                SetScreenBounds();
            }

            yield return m_waitForSeconds;
        }
    }
}
