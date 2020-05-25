using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class MulSpriteAtlas : MonoBehaviour
{
    public List<SpriteAtlas> mAtlases = new List<SpriteAtlas>();
    Dictionary<string, int> mAtlasIndices = new Dictionary<string, int>();

    public SpriteAtlas GetSpriteAtlas(string name)
    {
        if (mAtlases.Count == 0) {
            return null;
        }

        if (mAtlases.Count == 0) {
            for (int i = 0; i < mAtlases.Count; i++) {
                mAtlases[i].Init(ref mAtlasIndices, i);
            }
        }

        int index;
        if (mAtlasIndices.TryGetValue(name, out index)){
            if (index > -1 && index < mAtlases.Count) {
                return mAtlases[index];
            }

            return mAtlasIndices.TryGetValue(name, out index) ? mAtlases[index] : null;
        }
        return null;
    }

    public Material GetMaterial(string name)
    {
        SpriteAtlas atlas = GetSpriteAtlas(name);
        if (atlas != null) {
            return atlas.GetMaterial();
        }
        return null;
    }

    public Material GetClipMaterial(string name)
    {
        SpriteAtlas atlas = GetSpriteAtlas(name);
        if ( atlas != null){
            return atlas.GetClipMaterial();
        }
        return null;
    }

    public Sprite GetSprite(string name)
    {
        SpriteAtlas atlas = GetSpriteAtlas(name);
        if (atlas != null){
            return atlas.GetSprite(name);
        }
        return null;
    }



}