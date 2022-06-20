using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions
{
    public static class LeanTweenExtensions
    {
        public static LTDescr LeanSliderValue(this Slider slider, float value, float time = .3f,
            LeanTweenType easeType = LeanTweenType.easeOutExpo)
        {
            slider.gameObject.LeanCancel();

            return slider.gameObject.LeanValue
            (
                val => { slider.value = val; },
                slider.value,
                value,
                time
            ).setEase(easeType);
        }

        public static LTDescr LeanFillValue(this Image image, float value, float time = .3f,
            LeanTweenType easeType = LeanTweenType.easeOutExpo)
        {
            image.gameObject.LeanCancel();

            return image.gameObject.LeanValue
            (
                val => image.fillAmount = val,
                image.fillAmount,
                value,
                time
            ).setEase(easeType);
        }

        public static LTDescr LeanColor(this Graphic graphic, Color newColor, float time = .3f)
        {
            GameObject gameObject;

            (gameObject = graphic.gameObject).LeanCancel();

            return gameObject.LeanValue
            (
                val => graphic.color = val,
                graphic.color,
                newColor,
                time
            ).setEaseOutExpo();
        }

        public static LTDescr LeanColor(this Graphic graphic, Color fromColor, Color newColor, float time = .3f)
        {
            GameObject gameObject;

            (gameObject = graphic.gameObject).LeanCancel();

            return gameObject.LeanValue
            (
                val => graphic.color = val,
                fromColor,
                newColor,
                time
            ).setEaseOutExpo();
        }

        public static LTDescr LeanDelayedText(this TMP_Text label, float time = .7f)
        {
            GameObject gameObject;

            (gameObject = label.gameObject).LeanCancel();

            return gameObject.LeanValue
            (
                val => label.maxVisibleCharacters = (int)val,
                0,
                label.text.Length,
                time
            ).setEaseOutQuad();
        }

        public static LTDescr LeanTickerValue(this TMP_Text label, int from, int value, float time = .3f)
        {
            GameObject gameObject;

            (gameObject = label.gameObject).LeanCancel();

            return gameObject.LeanValue
            (
                val => label.text = val.ToString("0"),
                from,
                value,
                time
            ).setEaseOutExpo();
        }

        public static LTDescr LeanHorizontalScroll(this ScrollRect scrollRect, float value, float time = .2f)
        {
            GameObject gameObject;

            (gameObject = scrollRect.gameObject).LeanCancel();

            return gameObject.LeanValue
            (
                val => scrollRect.horizontalNormalizedPosition = val,
                scrollRect.horizontalNormalizedPosition,
                value,
                time
            ).setEaseOutExpo();
        }

        public static LTDescr LeanHorizontalShake(this Transform transform, float time = .2f, float fromOffset = -3.5f)
        {
            if (!transform) return null;

            var position = transform.position;

            return transform.LeanMoveX(position.x, time)
                .setFrom(position.x + fromOffset)
                .setOnComplete(() => transform.LeanMoveX(position.x, .1f))
                .setEaseShake();
        }

        public static LTDescr LeanHorizontalShakeLocal(this Transform transform, float time = .2f,
            float fromOffset = -3.5f)
        {
            if (!transform) return null;

            return transform.LeanMoveLocalX(0f, time)
                .setFrom(fromOffset + fromOffset)
                .setOnComplete(() => transform.LeanMoveLocalX(0f, .1f))
                .setEaseShake();
        }

        // public static LTDescr LeanHorizontalShake(this Transform transform, Vector3 finalPosition, float time = .2f,
        //     float fromOffset = -3.5f)
        // {
        //     if (!transform) return null;
        //
        //     return transform.LeanMoveLocalX(0f, time)
        //         .setFrom(fromOffset)
        //         .setOnComplete(() => transform.LeanMoveLocalX(0f, .1f))
        //         .setEaseShake();
        // }

        public static LTDescr LeanHorizontalShake(this GameObject gameObject, float time = .2f,
            float fromOffset = -3.5f)
        {
            return gameObject.transform.LeanHorizontalShake(time, fromOffset);
        }

        public static LTDescr LeanHorizontalShake(this GameObject gameObject, Vector3 finalPosition, float time = .2f,
            float fromOffset = -3.5f)
        {
            return gameObject.transform.LeanHorizontalShake(finalPosition, time, fromOffset);
        }

        public static LTDescr LeanHorizontalShake(this Transform transform, Vector3 startPosition,
            float startTime = .2f, float endTime = .1f, float fromOffset = -3.5f)
        {
            if (!transform) return null;

            var position = transform.position;

            return transform.LeanMoveX(position.x + fromOffset, startTime)
                .setFrom(startPosition.x)
                .setOnComplete(() => transform.LeanMoveX(startPosition.x, endTime))
                .setEaseShake();
        }

        public static LTDescr LeanRotationShake(this Transform transform, float time = .2f, float fromOffset = -3.5f)
        {
            if (!transform) return null;

            var rotation = transform.rotation;

            return transform.LeanRotateZ(rotation.x, time)
                .setFrom(rotation.x + fromOffset)
                .setOnComplete(() => transform.LeanRotateZ(rotation.x, .1f))
                .setEaseShake();
        }

        public static LTDescr LeanRotationShakeLocal(this Transform transform, float time = .2f,
            float fromOffset = -3.5f)
        {
            if (!transform) return null;

            return transform.LeanRotateZ(0f, time)
                .setFrom(fromOffset)
                .setOnComplete(() => transform.LeanRotateZ(0f, .1f))
                .setEaseShake()
                .setLoopCount(2);
        }

        public static LTDescr LeanAlpha(this Graphic graphic, float to, float time = .3f)
        {
            return graphic.LeanAlpha(graphic.color.a, to, time);
        }

        public static LTDescr LeanAlpha(this Graphic graphic, float from, float to, float time = .3f)
        {
            GameObject gameObject;

            (gameObject = graphic.gameObject).LeanCancel();

            return gameObject.LeanValue
            (
                val =>
                {
                    var color = graphic.color;

                    color.a = val;

                    graphic.color = color;
                },
                from,
                to,
                time
            ).setEaseOutExpo();
        }

        public static LTDescr LeanAlpha(this SpriteRenderer spriteRenderer, float from, float to, float time = .3f)
        {
            GameObject gameObject;

            (gameObject = spriteRenderer.gameObject).LeanCancel();

            return gameObject.LeanValue
            (
                val =>
                {
                    var color = spriteRenderer.color;

                    color.a = val;

                    spriteRenderer.color = color;
                },
                from,
                to,
                time
            ).setEaseOutExpo();
        }
    }
}