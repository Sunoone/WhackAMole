using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Tweens {
    [System.Serializable]
    public struct MoveTweenData
    {
        public TweenType TweenType;
        public Vector3 EndPosition;
        public float Duration;

        public MoveTweenData(TweenType tweenType, Vector3 endPosition, float duration)
        {
            TweenType = tweenType;
            EndPosition = endPosition;
            Duration = duration;
        }
    }
}