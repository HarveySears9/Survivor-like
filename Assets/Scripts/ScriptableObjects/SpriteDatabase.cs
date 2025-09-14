using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDatabase", menuName = "Game/SpriteDatabase")]
public class SpriteDatabase : ScriptableObject
{
    [Header("Brick Skins")]
    public Sprite[] red, redMoving, redDead;
    public Sprite[] blue, blueMoving, blueDead;
    public Sprite[] black, blackMoving, blackDead;
    public Sprite[] gold, goldMoving, goldDead;
    public Sprite[] teal, tealMoving, tealDead;
}
