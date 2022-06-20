using System;
using UnityEngine;

namespace DesertImage.UI
{
    public class SlideTransition : ATransition
    {
        private enum Direction
        {
            Left,
            Right,
            Top,
            Bottom
        }

        [SerializeField] private Direction direction;
        [SerializeField] private bool invert;

        [SerializeField] private float duration = 1f;
        [SerializeField] private float delay;

        [SerializeField] private LeanTweenType easeType;

        private const float Offset = 100f;

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
            IsInProcess = true;

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

            _rectTransform
                .LeanMove((Vector3)(invert ? startPosition : _defaultPosition), duration)
                .setFrom(invert ? _defaultPosition : startPosition)
                .setDelay(delay)
                .setEase(easeType)
                .setOnComplete(() =>
                {
                    IsInProcess = false;

                    callback?.Invoke();
                })
                .setUseEstimatedTime(true);
        }

        public override void ShowHard(Transform transform, Action callback = null)
        {
            var targetPosition = _defaultPosition;

            if (invert)
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

            var rectTransform = transform as RectTransform;

            rectTransform.anchoredPosition = targetPosition;

            callback?.Invoke();
        }

        private Vector3 NegativeCompensation()
        {
            var sizeDelta = /*_rectTransform.sizeDelta == Vector2.zero
                ? */_rectTransform.rect.size
                /*: _rectTransform.sizeDelta*/;

            var pivot = _rectTransform.pivot;

            return new Vector2(-sizeDelta.x + sizeDelta.x * pivot.x, -sizeDelta.y + sizeDelta.y * pivot.y) +
                   new Vector2(-Offset, -Offset);
        }

        private Vector3 PositiveCompensation()
        {
            var sizeDelta = /*_rectTransform.sizeDelta == Vector2.zero
                ? */_rectTransform.rect.size
                /*: _rectTransform.sizeDelta*/;

            var pivot = _rectTransform.pivot;

            return new Vector2(sizeDelta.x * pivot.x, sizeDelta.y * pivot.y) + new Vector2(Offset, Offset);
        }

        private Vector2 EndPosition(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    return (Vector3)(_bottomLeftCoord + _defaultPosition) + NegativeCompensation();

                case Direction.Right:
                    return (Vector3)(_topRightCoord + _defaultPosition) + PositiveCompensation();

                case Direction.Top:
                    return (Vector3)(_topRightCoord + _defaultPosition) + PositiveCompensation();

                case Direction.Bottom:
                    return (Vector3)(_bottomLeftCoord + _defaultPosition) + NegativeCompensation();
            }

            return _defaultPosition;
        }

        public override void Cancel()
        {
            IsInProcess = false;

            if (!_transform) return;

            _transform.gameObject.LeanCancel();
        }
    }
}