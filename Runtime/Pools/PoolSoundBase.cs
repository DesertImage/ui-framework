﻿using DesertImage.Audio;using UnityEngine;namespace DesertImage.Pools{    public class PoolSoundBase : SimpleMonoPool<SoundBase>    {        public PoolSoundBase(Transform parent) : base(parent)        {        }        protected override SoundBase CreateInstance()        {            var newObj = new GameObject("SoundFx");                        newObj.AddComponent<AudioSource>();                        return newObj.AddComponent<SoundBase>();        }    }}