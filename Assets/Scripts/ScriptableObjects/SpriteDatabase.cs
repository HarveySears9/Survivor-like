using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDatabase", menuName = "Game/SpriteDatabase")]
public class SpriteDatabase : ScriptableObject
{

    [Header("Skin names")]
    public string[] skinNames;

    [Header("Brick Skins")]
    public Sprite[] red, redMoving, redDead;
    public Sprite[] blue, blueMoving, blueDead;
    public Sprite[] black, blackMoving, blackDead;
    public Sprite[] gold, goldMoving, goldDead;
    public Sprite[] teal, tealMoving, tealDead;
    public Sprite[] bone, boneMoving, boneDead;
    public Sprite[] suit, suitMoving, suitDead;
    public Sprite[] blackBone, blackBoneMoving, blackBoneDead;
    public Sprite[] whiteSuit, whiteSuitMoving, whiteSuitDead;
    public Sprite[] blackSuit, blackSuitMoving, blackSuitDead;
    public Sprite[] chef, chefMoving, chefDead;
}
