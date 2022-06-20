using System;
using UnityEngine;

namespace DesertImage.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AlphaSlideTransition : ATransition
    {
        private enum Direction
        {
            Left,
            Right,
            Top,
            Bottom
        }

        [SerializeField] [Header(("Slide"))] private Direction direction;
        [SerializeField] private bool slideInvert;

        [SerializeField] private float slideDuration = 1f;
        [SerializeField] private float slideDelay;

        [SerializeField] private LeanTweenType slideEaseType;

        [SerializeField] [Header(("Alpha"))] [Space(15)]
        private CanvasGroup canvasGroup;

        [SerializeField] private float alphaTo;
        [SerializeField] private float alphaFrom;

        [SerializeField] private float alphaDuration;
        [SerializeField] private float alphaDelay;

        [SerializeField] private LeanTweenType alphaEaseType;

        private Transform _transform;

        private Vector2 _defaultPosition;
        private RectTransform _rectTransform;
        private Vector2 _topRightCoord;
        private Vector2 _bottomLeftCoord;

        public override void Init(Transform transf)
        {
            base.Init(transf);

            _transform = transf;

            _rectTransform = transf as RectTransform;

            if (!UIManager.UICanvas) return;

            var uiRect = UIManager.UICanvas.transform as RectTransform;

            var rect = uiRect.rect;

            _defaultPosition = _rectTransform.anchoredPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                _rectTransform,
                new Vector2(Screen.width, Screen.height),
                UIManager.UICanvas.worldCamera,
                out _topRightCoord
            );

            RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                _rectTransform,
                new Vector2(0, 0),
                UIManager.UICanvas.worldCamera,
                out _bottomLeftCoord
            );
        }

        public override void Play(Transform transform, Action callback)
        {
            var startPosition = Vector2.zero;

            _rectTransform.LeanCancel();

            switch (direction)
            {
                case Direction.Left:
                    startPosition = new Vector2(EndPosition(Direction.Left).x, _rectTransform.anchoredPosition.y);
                    break;

                case Direction.Right:
                    startPosition = new Vector2(EndPosition(Direction.Right).x, _rectTransform.anchoredPosition.y);
                    break;

                case Direction.Top:
                    startPosition = new Vector2(_rectTransform.anchoredPosition.x, EndPosition(Direction.Top).y);
                    break;

                case Direction.Bottom:
                    startPosition =
                        new Vector2(_rectTransform.anchoredPosition.x, EndPosition(Direction.Bottom).y);
                    break;
            }

            _rectTransform.LeanMove((Vector3) (slideInvert ? startPosition : _defaultPosition), slideDuration)
                .setFrom(slideInvert ? _defaultPosition : startPosition)
                .setDelay(slideDelay)
                .setEase(slideEaseType)
                .setUseEstimatedTime(true);

            canvasGroup.LeanAlpha(alphaTo, alphaDuration)
                .setFrom(alphaFrom)
                .setDelay(alphaDelay)
                .setOnComplete(callback)
                .setEase(alphaEaseType)
                .setUseEstimatedTime(true);
        }

        public override void ShowHard(Transform transform, Action callback = null)
        {
            var targetPosition = _defaultPosition;

            if (slideInvert)
            {
                var startPosition = Vector2.zero;

                switch (direction)
                {
                    case Direction.Left:
                        startPosition = new Vector2(EndPosition(Direction.Left).x, _rectTransform.anchoredPosition.y);
                        break;

                    case Direction.Right:
                        startPosition = new Vector2(EndPosition(Direction.Right).x, _rectTransform.anchoredPosition.y);
                        break;

                    case Direction.Top:
                        startPosition = new Vector2(_rectTransform.anchoredPosition.x, EndPosition(Direction.Top).y);
                        break;

                    case Direction.Bottom:
                        startPosition =
                            new Vector2(_rectTransform.anchoredPosition.x, EndPosition(Direction.Bottom).y);
                        break;
                }

                targetPosition = startPosition;
            }

            transform.position = targetPosition;

            callback?.Invoke();
        }

        private Vector3 NegativeCompensation()
        {
            var sizeDelta = _rectTransform.sizeDelta;
            var pivot = _rectTransform.pivot;

            return new Vector2((-sizeDelta.x) + sizeDelta.x * pivot.x, (-sizeDelta.y) + sizeDelta.y * pivot.y);
        }

        private Vector3 PositiveCompensation()
        {
            var sizeDelta = _rectTransform.sizeDelta;
            var pivot = _rectTransform.pivot;

            return new Vector2((sizeDelta.x * pivot.x), (sizeDelta.y * pivot.y));
        }

        private Vector2 EndPosition(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return (Vector3) (_bottomLeftCoord + _defaultPosition) + NegativeCompensation();

                case Direction.Right:
                    return (Vector3) (_topRightCoord + _defaultPosition) + PositiveCompensation();

                case Direction.Top:
                    return (Vector3) (_topRightCoord + _defaultPosition) + PositiveCompensation();

                case Direction.Bottom:
                    return (Vector3) (_bottomLeftCoord + _defaultPosition) + NegativeCompensation();
            }

            return _defaultPosition;
        }

        public override void Cancel()
        {
            if (!_transform) return;

            _transform.gameObject.LeanCancel();
        }

        private void OnValidate()
        {
            if (!canvasGroup)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        private void OnDestroy()
        {
            canvasGroup = null;
        }
    }
}