using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "Puzzle Bobble/Characters/New Character")]
public class PB_CharacterDataSO : ScriptableObject
{
    [SerializeField]
    private string _characterName = "";
    [SerializeField]
    private SpriteLibraryAsset _characterSpriteLib = null;

    public SpriteLibraryAsset GetCharacterSpriteLib()
    {
        return _characterSpriteLib;
    }

    public void SetCharacterSpriteLib(SpriteLibraryAsset spriteLibAsset)
    {
        _characterSpriteLib = spriteLibAsset;
    }
}
