using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_CreateModel : MonoBehaviour {

	private Image mask;
	private Canvas canvasWindow;
	private RectTransform Save, Atlas;
	private Button trueSaveButton, trueAtlasButton;
	private Text tipsText;
	private Vector3 vEnable = new Vector3 (0, 0, 0);
	private Vector3 vUnEnable = new Vector3 (-1000, -1000, 0);
	private string theme;
	private string UIColor;
	private string textColor;
	private string pathUI;
	private float time = 0.0f;

	// 初始化调用
	void Start () {
		// 设置遮罩和弹出窗口的可见性
		mask = GameObject.Find ("Canvas/Mask/mask").GetComponent<Image> ();
		mask.enabled = false;
		canvasWindow = GameObject.Find ("CanvasWindow").GetComponent<Canvas> ();
		canvasWindow.enabled = false;
		Save = canvasWindow.transform.Find ("Save").GetComponent<RectTransform> ();
		Atlas = canvasWindow.transform.Find ("Atlas").GetComponent<RectTransform> ();
		Save.localPosition = vUnEnable;
		Atlas.localPosition = vUnEnable;

		InitializeUIColor ();
	}

	void Update(){
		time += Time.deltaTime;
	}

	private void LocalPositionBut (RectTransform but) {
		// min是左下(left,bottom),max是右上(right,top),如果Anchor max=(1,1)，则max要传入负值
		but.offsetMin = new Vector2 (72, 11);
		but.offsetMax = new Vector2 (-288, -249);
	}

	// 返回
	public void Button_Return () {
		GameObject.Find ("Main Camera").GetComponent<TangramActionsInCreate> ().enabled = true;
		mask.enabled = false;
		trueSaveButton.GetComponent<RectTransform> ().localPosition = vUnEnable;
		trueAtlasButton.GetComponent<RectTransform> ().localPosition = vUnEnable;
		Save.localPosition = vUnEnable;
		Atlas.localPosition = vUnEnable;
	}

	// 返回-跳转场景
	public void Button_Exit () {
		PlayerPrefs.SetFloat ("TimeChange", time);
		SceneManager.LoadScene ("MainSence");
	}

	// 图鉴-跳转场景
	public void Button_Atlas () {
		PlayerPrefs.SetFloat ("TimeChange", time);
		SceneManager.LoadScene ("Atlas");
	}

	// 确定保存的二次确认
	public void Button_IsTrueSave () {
		GameObject.Find ("Main Camera").GetComponent<TangramActionsInCreate> ().enabled = false;
		mask.enabled = true;
		canvasWindow.enabled = true;
		Save.localPosition = vEnable;
		Atlas.localPosition = vUnEnable;
		tipsText.text = (textColor + "确定要保存当前图案吗？" + "</color>");
		LocalPositionBut(trueSaveButton.GetComponent<RectTransform> ());
	}

	// 确定保存
	public void Button_Save () {
		GameObject.Find ("Main Camera").GetComponent<TangramActionsInCreate> ().enabled = false;
		mask.enabled = true;
		canvasWindow.enabled = true;
		Save.localPosition = vEnable;
		tipsText.text = (textColor + "保存成功！要前往宝库查看吗？" + "</color>");
		trueSaveButton.GetComponent<RectTransform> ().localPosition = vUnEnable;
		LocalPositionBut(trueAtlasButton.GetComponent<RectTransform> ());

	}

	// 查看图鉴的二次确认
	public void Button_IsTureAtlas () {
		GameObject.Find ("Main Camera").GetComponent<TangramActionsInCreate> ().enabled = false;
		mask.enabled = true;
		canvasWindow.enabled = true;
		Atlas.localPosition = vEnable;
		Save.localPosition = vUnEnable;
	}

	void OnApplicationQuit(){
		float time = Time.deltaTime;
		PlayerPrefs.SetFloat ("TimeChange", time);
		System.DateTime gameExitTime = System.DateTime.Now;
		PlayerPrefs.SetInt ("GamgExitTimeDay", (int)gameExitTime.DayOfYear);
		PlayerPrefs.SetInt ("GamgExitTimeHour", (int)gameExitTime.Hour);
		PlayerPrefs.SetInt ("GamgExitTimeMinute", (int)gameExitTime.Minute);
		PlayerPrefs.SetInt ("GamgExitTimeSecond", (int)gameExitTime.Second);
		Application.Quit ();
	}

	void InitializeUIColor () {
		theme = PlayerPrefs.GetString ("theme");
		UIColor = PlayerPrefs.GetString ("UIColor");
		textColor = PlayerPrefs.GetString ("textColor");
		pathUI = "Sprites/" + theme + "/UI_" + UIColor + "/UI_" + UIColor;

		GameObject.Find ("Background/Background").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Background01");
		GameObject.Find ("Background/Dividinglineh").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("Background/Dividinglines").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		// Canvas下组件的初始化
		GameObject.Find ("Canvas/Mask/SaveButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonSave");
		GameObject.Find ("Canvas/Mask/SaveButton/Text").GetComponent<Text> ().text = (textColor + "       保存图案" + "</color>");
		GameObject.Find ("Canvas/Mask/AtlasButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonAtlas");
		GameObject.Find ("Canvas/Mask/AtlasButton/Text").GetComponent<Text> ().text = (textColor + "       前往图鉴" + "</color>");
		GameObject.Find ("Canvas/Mask/ExitButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("Canvas/Mask/ExitButton/Text").GetComponent<Text> ().text = (textColor + "       退 出" + "</color>");

		// CancasWindow下组件的初始化
		GameObject.Find ("CanvasWindow/Save").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_Window01");
		GameObject.Find ("CanvasWindow/Save/isSave").GetComponent<Text> ().text = (textColor + "保存" + "</color>");
		tipsText = GameObject.Find ("CanvasWindow/Save/TipsText").GetComponent<Text> ();
		tipsText.text = (textColor + "确定要保存当前图案吗？" + "</color>");
		trueSaveButton = GameObject.Find ("CanvasWindow/Save/TrueSaveButton").GetComponent<Button> ();
		trueSaveButton.GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		trueSaveButton.transform.Find("Text").GetComponent<Text>().text = (textColor + "确定" + "</color>");
		trueSaveButton.GetComponent<RectTransform> ().localPosition = vUnEnable;
		trueAtlasButton = GameObject.Find ("CanvasWindow/Save/TrueAtlasButton").GetComponent<Button> ();
		trueAtlasButton.GetComponent<Image>().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		trueAtlasButton.transform.Find("Text").GetComponent<Text>().text = (textColor + "前往宝库" + "</color>");
		trueAtlasButton.GetComponent<RectTransform> ().localPosition = vUnEnable;
		GameObject.Find ("CanvasWindow/Save/ReturnButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		GameObject.Find ("CanvasWindow/Save/ReturnButton/Text").GetComponent<Text> ().text = (textColor + "返回" + "</color>");

		GameObject.Find ("CanvasWindow/Atlas").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_Window01");
		GameObject.Find ("CanvasWindow/Atlas/TitleText").GetComponent<Text> ().text = (textColor + "前往宝库" + "</color>");
		GameObject.Find ("CanvasWindow/Atlas/TipsText").GetComponent<Text> ().text = (textColor + "确定要放弃当前图案，\r\n前往【宝库】吗？" + "</color>");
		GameObject.Find ("CanvasWindow/Atlas/TrueButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		GameObject.Find ("CanvasWindow/Atlas/TrueButton/Text").GetComponent<Text> ().text = (textColor + "确定前往" + "</color>");
		GameObject.Find ("CanvasWindow/Atlas/ReturnButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		GameObject.Find ("CanvasWindow/Atlas/ReturnButton/Text").GetComponent<Text> ().text = (textColor + "返回" + "</color>");
	}
}