using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteAtlas
{
    public Material material;
    public Material clipMaterial;

    public List<Sprite> mSprites = new List<Sprite>();
    // 实例化材质
    private Material _material;
    private Material _clipMaterial;
    private Dictionary<string, int> mSpriteIndices = new Dictionary<string, int>();

    public Material GetMaterial()
    {
        if(_material == null && material != null){
            #if (!UNITY_EDITOR && UNITY_STANDALONE) || UNITY_IOS || UNITY_ANDROID || XYD_BUNDLE
            _material = material;
            #else
            _material = UnityEngine.Object.Instantiate(material);
            #endif
        }
        return _material;
    }

    public Material GetClipMaterial()
    {
        if(_clipMaterial == null && clipMaterial != null){
            #if (!UNITY_EDITOR && UNITY_STANDALONE) || UNITY_IOS || UNITY_ANDROID || XYD_BUNDLE
            _clipMaterial = clipMaterial;
            #else
            _clipMaterial = UnityEngine.Object.Instantiate(clipMaterial);
            #endif
        }
        return _clipMaterial;
    }

    public Sprite GetSprite(string name)
    {
        if (mSpriteIndices.Count == 0){
            return null;
        }

        if (mSprites.Count != mSpriteIndices.Count){
            mSpriteIndices.Clear();
            for (int i = 0; i < mSprites.Count; ++i){
                mSpriteIndices[mSprites[i].name] = i;
            }
        }

        int index;
        if (mSpriteIndices.TryGetValue(name, out index)){
            if (index > -1 && index < mSprites.Count){
                return mSprites[index];
            }

            return mSpriteIndices.TryGetValue(name, out index) ? mSprites[index] : null;
        }
        return null;
    }

    // ---- 输入一个Dictionary 与index，
    public void Init(ref Dictionary<string, int> dict, int index)
    {
        if (mSprites.Count == 0){
            return;
        }

        if (mSprites.Count != mSpriteIndices.Count){
            mSpriteIndices.Clear();
            for (int i = 0; i < mSprites.Count; ++i){
                mSpriteIndices[mSprites[i].name] = i;
                dict[mSprites[i].name] = index;
            }
        }
    }


}