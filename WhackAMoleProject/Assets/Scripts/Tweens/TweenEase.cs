using UnityEngine;

namespace Tweens
{
    public enum TweenType
    {
        Linear,

        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,

        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,

        EaseInQuartic,
        EaseOutQuartic,
        EaseInOutQuartic,

        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,

        EaseInSine,
        EaseOutSine,
        EaseInOutSine,

        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,

        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc
    }
    public static class TweenEase
    {
        private delegate float Tween(float time, float startValue, float differenceValue, float duration);
        private static Tween[] _tweens = new Tween[] { Linear,
                                                       EaseInQuad, EaseOutQuad, EaseInOutQuad,
                                                       EaseInCubic, EaseOutCubic, EaseInOutCubic,
                                                       EaseInQuartic, EaseOutQuartic, EaseInOutQuartic,
                                                       EaseInQuint, EaseOutQuint, EaseInOutQuint,
                                                       EaseInSine, EaseOutSine, EaseInOutSine,
                                                       EaseInExpo, EaseOutExpo, EaseInOutExpo,
                                                       EaseInCirc, EaseOutCirc, EaseInOutCirc };

        public static float GetNewValue(TweenType tweenType, float time, float startValue, float differenceValue, float duration)
        {
            return _tweens[(int)tweenType].Invoke(time, startValue, differenceValue, duration);
        }

        public static float Linear(float time, float startValue, float differenceValue, float duration)
        {
            return ((differenceValue * time) / duration) + startValue;
        }

        public static float EaseInQuad(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration;
            return differenceValue * time * time + startValue;
        }
        public static float EaseOutQuad(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration;
            return -differenceValue * time * (time - 2) + startValue;
        }
        public static float EaseInOutQuad(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration / 2;
            if (time < 1) return differenceValue / 2 * time * time + startValue;
            time--;
            return -differenceValue / 2 * (time * (time - 2) - 1) + startValue;
        }

        public static float EaseInCubic(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration;
            return differenceValue * time * time * time + startValue;
        }
        public static float EaseOutCubic(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration;
            time--;
            return differenceValue * (time * time * time + 1) + startValue;
        }
        public static float EaseInOutCubic(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration / 2;
            if (time < 1) return differenceValue / 2 * time * time * time + startValue;
            time -= 2;
            return differenceValue / 2 * (time * time * time + 2) + startValue;
        }

        public static float EaseInQuartic(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration;
            return differenceValue * time * time * time * time + startValue;
        }
        public static float EaseOutQuartic(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration;
            time--;
            return -differenceValue * (time * time * time * time - 1) + startValue;
        }
        public static float EaseInOutQuartic(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration / 2;
            if (time < 1) return differenceValue / 2 * time * time * time * time + startValue;
            time -= 2;
            return -differenceValue / 2 * (time * time * time * time - 2) + startValue;
        }

        public static float EaseInQuint(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration;
            return differenceValue * time * time * time * time * time + startValue;
        }
        public static float EaseOutQuint(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration;
            time--;
            return differenceValue * (time * time * time * time * time + 1) + startValue;
        }
        public static float EaseInOutQuint(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration / 2;
            if (time < 1) return differenceValue / 2 * time * time * time * time * time + startValue;
            time -= 2;
            return differenceValue / 2 * (time * time * time * time * time + 2) + startValue;
        }

        public static float EaseInSine(float time, float startValue, float differenceValue, float duration)
        {
            return -differenceValue * Mathf.Cos(time / duration * (Mathf.PI / 2)) + differenceValue + startValue;
        }
        public static float EaseOutSine(float time, float startValue, float differenceValue, float duration)
        {
            return differenceValue * Mathf.Sin(time / duration * (Mathf.PI / 2)) + startValue;
        }
        public static float EaseInOutSine(float time, float startValue, float differenceValue, float duration)
        {
            return -differenceValue / 2 * (Mathf.Cos(Mathf.PI * time / duration) - 1) + startValue;
        }

        public static float EaseInExpo(float time, float startValue, float differenceValue, float duration)
        {
            return differenceValue * Mathf.Pow(2, 10 * (time / duration - 1)) + startValue;
        }
        public static float EaseOutExpo(float time, float startValue, float differenceValue, float duration)
        {
            return differenceValue * (-Mathf.Pow(2, -10 * time / duration) + 1) + startValue;
        }
        public static float EaseInOutExpo(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration / 2;
            if (time < 1) return differenceValue / 2 * Mathf.Pow(2, 10 * (time - 1)) + startValue;
            time--;
            return differenceValue / 2 * (-Mathf.Pow(2, -10 * time) + 2) + startValue;
        }

        public static float EaseInCirc(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration;
            return -differenceValue * (Mathf.Sqrt(1 - time * time) - 1) + startValue;
        }
        public static float EaseOutCirc(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration;
            time--;
            return differenceValue * Mathf.Sqrt(1 - time * time) + startValue;
        }
        public static float EaseInOutCirc(float time, float startValue, float differenceValue, float duration)
        {
            time /= duration / 2;
            if (time < 1) return -differenceValue / 2 * (Mathf.Sqrt(1 - time * time) - 1) + startValue;
            time -= 2;
            return differenceValue / 2 * (Mathf.Sqrt(1 - time * time) + 1) + startValue;
        }
    }
}
