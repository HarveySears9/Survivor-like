using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDatabase", menuName = "Game/SpriteDatabase")]
public class SpriteDatabase : ScriptableObject
{

    [Header("Skin names")]
    public string[] skinNames;

    [Header("Brick Skins")]
    public Sprite[] red, redMoving;
    public Sprite[] blue, blueMoving;
    public Sprite[] black, blackMoving;
    public Sprite[] gold, goldMoving;
    public Sprite[] teal, tealMoving;
    public Sprite[] bone, boneMoving;
    public Sprite[] suit, suitMoving;
    public Sprite[] blackBone, blackBoneMoving;
    public Sprite[] whiteSuit, whiteSuitMoving;
    public Sprite[] blackSuit, blackSuitMoving;
    public Sprite[] chef, chefMoving;
}
