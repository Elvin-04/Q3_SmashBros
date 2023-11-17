using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Parameter : MonoBehaviour
{
    public GameData resetedValues;

    public string parameter;
    public Slider slider;
    public TextMeshProUGUI value;
    public GameData gameData;

    private void Start()
    {
        Reset();
        SetValuesInit();
    }

    private void FixedUpdate()
    {
        SetValues();
    }

    private void SetValuesInit()
    {
        switch (parameter)
        {
            case "PlayerSpeed":
                slider.value = gameData.playerSpeed;
                value.text = gameData.playerSpeed.ToString();
                break;
            case "JumpCount":
                slider.value = gameData.jump;
                value.text = gameData.jump.ToString();
                break;
            case "JumpForce":
                slider.value = gameData.jumpForce;
                value.text = gameData.jumpForce.ToString();
                break;
            case "DodgeTime":
                slider.value = gameData.dodgeTime;
                value.text = gameData.dodgeTime.ToString();
                break;
            case "DashForce":
                slider.value = gameData.dodgeForce;
                value.text = gameData.dodgeForce.ToString();
                break;
            default:
                break;
        }
    }

    private void SetValues()
    {
        switch (parameter)
        {
            case "PlayerSpeed":
                gameData.playerSpeed = slider.value;
                value.text = gameData.playerSpeed.ToString("F2");
                break;
            case "JumpCount":
                gameData.jump = (int)slider.value;
                value.text = gameData.jump.ToString();
                break;
            case "JumpForce":
                gameData.jumpForce = slider.value;
                value.text = gameData.jumpForce.ToString("F2");
                break;
            case "DodgeTime":
                gameData.dodgeTime = slider.value;
                value.text = gameData.dodgeTime.ToString("F2");
                break;
            case "DashForce":
                gameData.dodgeForce = slider.value;
                value.text = gameData.dodgeForce.ToString("F2");
                break;
            default:
                break;
        }
    }

    public void Reset()
    {
        gameData.playerSpeed = resetedValues.playerSpeed;
        gameData.jump = resetedValues.jump;
        gameData.jumpForce = resetedValues.jumpForce;
        gameData.dodgeTime = resetedValues.dodgeTime;
        gameData.dodgeForce = resetedValues.dodgeForce;
        SetValuesInit();
    }
}
