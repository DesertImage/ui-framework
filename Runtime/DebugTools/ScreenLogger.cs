using System.Collections;
using UnityEngine;

namespace DesertImage.DebugTools
{
    public class ScreenLogger : MonoBehaviour
    {
        private string _myLog;
        private readonly Stack _myLogQueue = new Stack();

        private string _filter = "";

        private float _time;

        private void OnEnable()
        {
            if (!Debug.isDebugBuild)
            {
#if !SCREEN_LOGGER
                Destroy(this);

                return;
#endif
            }

            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (!logString.Contains(_filter) && type != LogType.Exception /*&& type != LogType.Error*/) return;

            _myLog = logString;

            var newString = "\n [" + type + "] : " + _myLog;

            if (type == LogType.Exception)
            {
                newString = "\n" + stackTrace;
            }

            _myLogQueue.Push(newString);

            RefreshLog();
        }

        private void RefreshLog()
        {
            _myLog = string.Empty;

            foreach (string myLog in _myLogQueue)
            {
                _myLog += myLog;
            }

            _time = 0f;
        }

        private void OnGUI()
        {
            var style = new GUIStyle {normal = {textColor = Color.blue}, fontSize = 20};

            GUILayout.Label(_myLog, style);
        }

        private void Update()
        {
            _time += Time.deltaTime;

            if (_time < 8f) return;

            _time = 0f;

            if (_myLogQueue.Count == 0) return;
            if (_myLogQueue.Peek() == null) return;

            _myLogQueue.Pop();

            RefreshLog();
        }
    }
}