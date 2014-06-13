﻿using System;

namespace Card.Effect
{
    [Serializable]
    public class EffectDefine
    {
        /// <summary>
        /// 描述
        /// </summary>
        public String 描述 = String.Empty;
        /// <summary>
        /// 效果条件
        /// </summary>
        public String 效果条件 = String.Empty;
        /// <summary>
        /// 效果条件为 真
        /// </summary>
        public AtomicEffectDefine TrueAtomicEffect;
        /// <summary>
        /// 效果条件为 伪
        /// </summary>
        public AtomicEffectDefine FalseAtomicEffect;

    }
}