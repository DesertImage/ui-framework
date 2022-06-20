using System;
using System.Collections.Generic;
using System.Reflection;
using DesertImage.UI;
using DesertImage.UI.Panel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    [CustomEditor(typeof(UISetup))]
    public class UISetupEditor : Editor
    {
        private static UISetup _target;

        private void OnEnable()
        {
            _target = (UISetup)target;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Build"))
            {
                Build(_target);
            }

            GUILayout.Space(15);

            base.OnInspectorGUI();
        }

        private static T GetPrivateField<T>(object targetObj, string fieldName) where T : class
        {
            return targetObj
                .GetType()
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(targetObj) as T;
        }

        private static void SetPrivateField(object targetObj, string fieldName, object value)
        {
            targetObj
                .GetType()
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(targetObj, value);
        }

        public static void Build(IUISetup uiSetup)
        {
            var manager = GetPrivateField<UIManager>(uiSetup, "uiManager");
            var screens = GetPrivateField<GameObject[]>(uiSetup, "screens");

            var uiManager = PrefabUtility.InstantiatePrefab(manager) as UIManager;

            foreach (var screen in screens)
            {
                var obj = PrefabUtility.InstantiatePrefab(screen, uiManager.transform) as GameObject;

                if (!obj) continue;

                var newScreen = obj.GetComponent<IScreen<ushort>>();

                uiManager.Register(newScreen.Id, newScreen);
            }

            try
            {
                uiManager.ShowAll();
            }
            catch (Exception e)
            {
#if DEBUG
                UnityEngine.Debug.LogError("[UISetupEditor] exception " + e);
#endif
            }

            var parent = uiManager.transform as RectTransform;

            #region MAKET

            var maket = new GameObject("Maket", typeof(RectTransform))
            {
                transform =
                {
                    parent = parent
                }
            };

            var maketImage = maket.AddComponent<Image>();
            maketImage.sprite = GetPrivateField<Sprite>(uiSetup, "maket");

            var maketImageColor = maketImage.color;
            maketImageColor.a = .5f;
            maketImage.color = maketImageColor;


            var maketRect = maket.transform as RectTransform;

            maketRect.transform.localScale = Vector3.one;
            maketRect.transform.SetParent(parent);

            maketRect.anchoredPosition = new Vector2();

            maketRect.anchorMin = new Vector2(0f, 0f);
            maketRect.anchorMax = new Vector2(1f, 1f);

            maketRect.pivot = new Vector2(0.5f, 0.5f);

            // maketRect.sizeDelta = parent.sizeDelta * .5f;

            maketRect.offsetMin = maketRect.offsetMax = new Vector2(0, 0);
            // maketRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            // maketRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            // maketRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            // maketRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);

            maket.SetActive(false);

            #endregion

            // var adaptiveSafeArea = uiManager.GetComponentInChildren<AdaptiveSafeArea>();
            // if (adaptiveSafeArea)
            // {
            //     adaptiveSafeArea.Resize(new Rect(132, 63, 2172, 1062));
            // }

            uiManager.gameObject.tag = "EditorOnly";
        }

        public static UISetupBase ConvertToSlots(UISetup uiSetup,
            Func<GameObject, IScreen, IScreenSlot> slotSetupAction = null)
        {
            const string PrefabsPath = "Assets/0 Content/1 Prefabs/0 UI/";

            var uiManager = PrefabUtility.InstantiatePrefab
            (
                GetPrivateField<UIManager>(uiSetup, "uiManager")
            ) as UIManager;

            uiManager.gameObject.AddComponent<RectTransform>();
            uiManager.transform.localScale = Vector3.one;

            var slotsList = new List<ScreenSlotBase>();

            var uiTemplateObj = new GameObject(uiSetup.name + "Template", typeof(RectTransform))
            {
                transform =
                {
                    localScale = Vector3.one
                }
            };
            uiTemplateObj.transform.SetParent(uiManager.transform);

            var uiTemplateRect = uiTemplateObj.transform as RectTransform;
            uiTemplateRect.anchorMin = new Vector2(0f, 0f);
            uiTemplateRect.anchorMax = new Vector2(1f, 1f);

            uiTemplateRect.pivot = new Vector2(.5f, .5f);

            uiTemplateRect.offsetMin = uiTemplateRect.offsetMax = new Vector2(0, 0);

            var screens = GetPrivateField<GameObject[]>(uiSetup, "screens");

            var uiTemplate = uiTemplateObj.AddComponent<UITemplate>();

            foreach (var obj in screens)
            {
                var screen = obj.GetComponent<ScreenBase>();

                if (!screen) continue;

                var slotObj = new GameObject(screen.name + "Slot", typeof(RectTransform));
                slotObj.transform.SetParent(uiTemplate.transform);

                IScreenSlot slot;

                if (slotSetupAction != null)
                {
                    slot = slotSetupAction.Invoke(slotObj, screen);
                }
                else
                {
                    switch (screen)
                    {
                        case IPanel _:
                            slot = slotObj.AddComponent<PanelSlotBase>();
                            break;

                        case IWindow window:
                            if (window.IsPopup)
                            {
                                slot = slotObj.AddComponent<PopupSlotBase>();
                            }
                            else
                            {
                                slot = slotObj.AddComponent<WindowSlotBase>();
                            }

                            break;

                        default:
                            slot = slotObj.AddComponent<WindowSlotBase>();
                            break;
                    }
                }

                var newId = screen.GetType()
                    .GetProperty
                    (
                        "Id",
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                    )
                    ?.GetValue(screen);

                var priority = GetPrivateField<object>(screen, "priority");

                SetPrivateField
                (
                    slot,
                    "id",
                    newId
                );

                SetPrivateField
                (
                    slot,
                    "priority",
                    priority
                );

                var fieldInfo = slot.GetType()
                    .GetField
                    (
                        "screen",
                        BindingFlags.NonPublic | BindingFlags.Instance
                    );

                slot.SetScreenInstance(screen, true);

                var enterTransition = GetPrivateField<ATransition>(screen, "enterTransition");
                var exitTransition = GetPrivateField<ATransition>(screen, "exitTransition");

                if (enterTransition != null)
                {
                    SetPrivateField
                    (
                        slot,
                        "enterTransition",
                        GetCopyOf(slotObj.AddComponent<SlideTransition>(), enterTransition)
                    );
                }

                if (exitTransition != null)
                {
                    SetPrivateField
                    (
                        slot,
                        "exitTransition",
                        GetCopyOf(slotObj.AddComponent<SlideTransition>(), exitTransition)
                    );
                }

                var slotBase = (ScreenSlotBase)slot;

                var slotRectTransform = slotBase.transform as RectTransform;
                var screenRectTransform = screen.transform as RectTransform;

                slotRectTransform.pivot = screenRectTransform.pivot;
                slotRectTransform.offsetMin = screenRectTransform.offsetMin;
                slotRectTransform.offsetMax = screenRectTransform.offsetMax;
                slotRectTransform.anchorMin = screenRectTransform.anchorMin;
                slotRectTransform.anchorMax = screenRectTransform.anchorMax;
                slotRectTransform.sizeDelta = screenRectTransform.sizeDelta;

                slotBase.transform.localScale = Vector3.one;

                slotsList.Add(slot as ScreenSlotBase);
            }

            SetPrivateField(uiTemplate, "slots", slotsList.ToArray());

            var path = $"{PrefabsPath}{uiSetup.name}Template.prefab";

            path = AssetDatabase.GenerateUniqueAssetPath(path);

            var prefab =
                PrefabUtility.SaveAsPrefabAssetAndConnect(uiTemplate.gameObject, path, InteractionMode.UserAction);

            uiTemplate = prefab.GetComponent<UITemplate>();

            DestroyImmediate(uiManager.gameObject);

            const string UITemplatesPath = "Assets/0 Content/3 Data/0 UI/";

            var uiTemplateSetup = CreateInstance<UITemplateSetup>();

            SetPrivateField(uiTemplateSetup, "uiManager", GetPrivateField<UIManager>(uiSetup, "uiManager"));
            SetPrivateField(uiTemplateSetup, "uiTemplate", uiTemplate);

            var assetPath = UITemplatesPath + $"/{uiSetup.name}Slots.asset";

            AssetDatabase.CreateAsset(uiTemplateSetup, assetPath);

            return uiTemplateSetup;
        }

        public static T GetCopyOf<T>(Component comp, T other) where T : Component
        {
            var type = comp.GetType();

            if (type != other.GetType()) return null; // type mis-match

            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                       BindingFlags.Default |
                                       BindingFlags.DeclaredOnly;

            var propertyInfos = type.GetProperties(flags);
            foreach (var infos in propertyInfos)
            {
                if (!infos.CanWrite) continue;

                try
                {
                    infos.SetValue(comp, infos.GetValue(other, null), null);
                }
                catch
                {
                    // ignored
                }
            }

            var fieldInfos = type.GetFields(flags);
            foreach (var info in fieldInfos)
            {
                info.SetValue(comp, info.GetValue(other));
            }

            return comp as T;
        }
    }
}