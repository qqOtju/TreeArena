using UnityEngine;

namespace Project.Scripts.Utils
{
    public class FPSCounter: MonoBehaviour
    {
        private float _fps;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void OnGUI()
        {
            _fps = 1.0f / Time.deltaTime;
            GUI.skin.label.fontSize = 40;
            GUILayout.Label("FPS: " + (int)_fps);
        }
    }
}