using Assets.Scripts.Services;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour {

	public Button registerButton;
	public Button loginButton;

	public InputField loginUserText;
	public InputField loginPswText;

	public InputField registerUserText;
	public InputField registerPswText;
    public InputField registerConfirmPswText;
    // Use this for initialization
    void Start () {
		registerButton.onClick.AddListener(OnClickRegister);
		loginButton.onClick.AddListener(OnClickLogIn);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(this.registerUserText.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.registerPswText.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        if (string.IsNullOrEmpty(this.registerConfirmPswText.text))
        {
            MessageBox.Show("请输入确认密码");
            return;
        }
        if (this.registerPswText.text != this.registerConfirmPswText.text)
        {
            MessageBox.Show("两次输入的密码不一致");
            return;
        }

        UserService.Instance.SendRegister(registerUserText.text,registerPswText.text);
    }

	public void OnClickLogIn()
	{
        if (string.IsNullOrEmpty(this.loginUserText.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.loginPswText.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }

        UserService.Instance.SendLogin(loginUserText.text, loginPswText.text);
    }
}
