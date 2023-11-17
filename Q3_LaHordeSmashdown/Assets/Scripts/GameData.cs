using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameDatas", order = 1)]
public class GameData : ScriptableObject
{
    public float playerSpeed;
    public int jump;
    public float jumpForce;
    public float dodgeTime;
    public float dodgeForce;
}
