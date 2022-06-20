using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIFramework
{
    [InitializeOnLoad]
    public static class UIMaketSceneView
    {
#if UNITY_EDITOR
        private static bool IsMaketHighlighted;

        private static float PreviousAlpha;

        static UIMaketSceneView()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private static void HandleInput()
        {
            var controlID = GUIUtility.GetControlID(FocusType.Passive);

            var currentEvent = Event.current;

            var controlType = currentEvent.GetTypeForControl(controlID);

            switch (controlType)
            {
                case EventType.KeyDown when !IsMaketHighlighted:
                {
                    if (currentEvent.keyCode == KeyCode.M)
                    {
                        if (currentEvent.shift)
                        {
                            HideMaket();
                        }
                        else
                        {
                            HighlightMaket();
                        }

                        Event.current.Use();
                    }

                    break;
                }
                case EventType.KeyUp when IsMaketHighlighted:
                {
                    if (currentEvent.keyCode == KeyCode.M)
                    {
                        UnhighlightMaket();
                        Event.current.Use();
                    }

                    break;
                }
            }
        }

        private static void HighlightMaket()
        {
            FindMaketAndSetAlpha
            (
                1f,
                image => PreviousAlpha = image.color.a
            );

            IsMaketHighlighted = true;
        }

        private static void HideMaket()
        {
            FindMaketAndSetAlpha
            (
                0f,
                image => PreviousAlpha = image.color.a
            );

            IsMaketHighlighted = true;
        }

        private static void UnhighlightMaket()
        {
            FindMaketAndSetAlpha(PreviousAlpha);

            IsMaketHighlighted = false;
        }

        private static void FindMaketAndSetAlpha(float alpha, Action<Image> preAlphaCallback = null)
        {
            var maket = GameObject.Find("Maket");

            if (!maket) return;

            var image = maket.GetComponent<Image>();

            if (!image) return;

            preAlphaCallback?.Invoke(image);

            //unity not updating windows after alpha changin so it's just a workaround
            image.gameObject.SetActive(false);

            var imageColor = image.color;
            imageColor.a = alpha;
            image.color = imageColor;

            image.gameObject.SetActive(true);
        }

        private static void DisableUI()
        {
            var scene = SceneManager.GetActiveScene();

            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                if (!rootGameObject.CompareTag("EditorOnly")) continue;

                rootGameObject.SetActive(false);
            }
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            HandleInput();
        }

        private static void OnPlayModeChanged(PlayModeStateChange obj)
        {
            DisableUI();
        }

        private static void HierarchyWindowItemOnGUI(int instanceid, Rect selectionrect)
        {
            HandleInput();
        }

        private static void OnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            DisableUI();
        }
    }
#endif
}