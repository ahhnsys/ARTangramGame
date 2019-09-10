using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_MainSence : MonoBehaviour {
	private Image mask;
	private Canvas canvasWindow;
	public Text remainingTimeText, speedBonusText;
	private Text qBNumber;
	private RectTransform square;
	private Vector3 vEnable = new Vector3 (124, 68, 0);
	private Vector3 vUnEnable = new Vector3 (-1000, -1000, 0);
	private Text tangramCountText1;
	private Text tangramCountText2;
	private Text tangramCountText3;
	private Text tangramCountText4;
	private Text tangramCountText5;
	private Text tangramCountText6;
	private Text tangramCountText7;
	private int[] tangramCount = new int[8];
	int day;
	int hour;
	int minute = 0;
	int second = 0;
	float useSecond = 0;
	int timeInterval = 600;
	float speedBonus = 0;
	float timeStep = 1.0f;
	float timeSpend = 0.0f;
	float timeChange = 0.0f;

	private string theme;
	private string UIColor;
	private string textColor;
	private string textUsableColor;
	private string pathUI;

	// 初始化调用
	void Start () {
		mask = GameObject.Find ("Canvas/mask").GetComponent<Image> ();
		mask.enabled = false;
		canvasWindow = GameObject.Find ("CanvasWindow").GetComponent<Canvas> ();
		canvasWindow.enabled = false;
		timeInterval = PlayerPrefs.GetInt ("TimeInterval");
		timeChange = PlayerPrefs.GetFloat ("TimeChange");
		System.DateTime gameStartTime = System.DateTime.Now;
		useSecond = PlayerPrefs.GetFloat ("TimeSpend");
		if (timeChange > 0.0f) {
			useSecond += timeChange;
			int count = (int)useSecond / timeInterval;
			if (count != 0)
				ProduceTangram (count);
			useSecond = useSecond - count * timeInterval;
			PlayerPrefs.SetFloat ("TimeChange", 0.0f);
			PlayerPrefs.SetFloat ("TimeSpend", 0.0f);
		}
		speedBonus = PlayerPrefs.GetInt ("SpeedBonus") / 100.0f;
		timeStep = 1.0f / (speedBonus + 1);

		InitializeUIColor ();
		if (PlayerPrefs.GetInt ("GameExitTimeDay") != 0) {
			day = (int)gameStartTime.DayOfYear - PlayerPrefs.GetInt ("GameExitTimeDay");
			if (day >= 0) {
				int timeOutGame = (int)gameStartTime.Hour * 60 + (int)gameStartTime.Minute - PlayerPrefs.GetInt ("GameExitTimeHour") * 60 - PlayerPrefs.GetInt ("GameExitTimeMinute");
				if (timeOutGame > 240)
					timeOutGame = 240;
				int number = 0;
				number = timeOutGame / 10;
				ProduceTangram (number);
			}
		}
	}
	
	// 每一帧调用
	void Update () {
		minute = (timeInterval - (int)useSecond) / 60;
		second = timeInterval - (int)useSecond - minute * 60;
		remainingTimeText.text = (textColor + string.Format ("{0:D2}:{1:D2}", minute, second) + "</color>");
		timeSpend += Time.deltaTime;
		// 每过一个时间步长，当前累计的时间+1
		if (timeSpend >= timeStep) {
			if (timeSpend >= 2 * timeStep) {
				float count = timeSpend / timeStep;
				useSecond += (int)count;
			} else {
				useSecond++;
			}
			if (useSecond >= timeInterval) {
				ProduceTangram (1);
				minute = 10;
				second = 0;
				remainingTimeText.text = (textColor + string.Format ("{0:D2}:{1:D2}", minute, second) + "</color>");
				useSecond = 0.0f;
			} else {
				minute = (timeInterval - (int)useSecond) / 60;
				second = timeInterval - (int)useSecond - minute * 60;
				remainingTimeText.text = (textColor + string.Format ("{0:D2}:{1:D2}", minute, second) + "</color>");
			}
			timeSpend = 0.0f;
		}
	}

	public void ProduceTangram (int number) {
		if (number > 0) {
			for (int i = 1; i <= number; i++) {
				int count = Random.Range (1, 8);
				tangramCount [count]++;
				PlayerPrefs.SetInt ("TangramCount" + count.ToString (), tangramCount [count]);
			}
		}
		tangramCountText1.text = (textUsableColor + tangramCount [1] + "</color>");
		tangramCountText2.text = (textUsableColor + tangramCount [2] + "</color>");
		tangramCountText3.text = (textUsableColor + tangramCount [3] + "</color>");
		tangramCountText4.text = (textUsableColor + tangramCount [4] + "</color>");
		tangramCountText5.text = (textUsableColor + tangramCount [5] + "</color>");
		tangramCountText6.text = (textUsableColor + tangramCount [6] + "</color>");
		tangramCountText7.text = (textUsableColor + tangramCount [7] + "</color>");
	}

	public void Button_MyTangram() {
		if (square.localPosition == vEnable)
			square.localPosition = vUnEnable;
		else
			square.localPosition = vEnable;
	}

	// 前往【闯关模式】场景
	public void Button_ForwardTangramAssemble () {
		bool isTangram = true;
		for (int i = 1; i <= 7; i++) {
			if (tangramCount [i] == 0) {
				isTangram = false;
				break;
			}
		}
		if (isTangram) {
			PlayerPrefs.SetFloat ("TimeSpend", useSecond);
			SceneManager.LoadScene ("TangramAssemble");
		} else {
			mask.enabled = true;
			canvasWindow.enabled = true;
		}
	}

	// 前往【自由模式】场景
	public void Button_ForwardCreateModel () {
		PlayerPrefs.SetFloat ("TimeSpend", useSecond);
		SceneManager.LoadScene ("CreateModel");
	}

	// 前往【宝库】场景
	public void Button_ForwardAtlas () {
		PlayerPrefs.SetFloat ("TimeSpend", useSecond);
		SceneManager.LoadScene ("Atlas");
	}

	// 前往【装扮】
	public void Button_ForwardSkin () {
		PlayerPrefs.SetFloat ("TimeSpend", useSecond);
		SceneManager.LoadScene ("Skin");
	}

	// 前往【设置】
	public void Button_ForwardSetting () {
		PlayerPrefs.SetFloat ("TimeSpend", second);
		SceneManager.LoadScene ("Setting");
	}

	// 返回
	public void Button_Return () {
		mask.enabled = false;
		canvasWindow.enabled = false;
	}

	// 退出游戏
	public void Button_Exit () {
		PlayerPrefs.SetFloat ("TimeSpend", useSecond);
		System.DateTime gameExitTime = System.DateTime.Now;
		PlayerPrefs.SetInt ("GamgExitTimeDay", (int)gameExitTime.DayOfYear);
		PlayerPrefs.SetInt ("GamgExitTimeHour", (int)gameExitTime.Hour);
		PlayerPrefs.SetInt ("GamgExitTimeMinute", (int)gameExitTime.Minute);
		PlayerPrefs.SetInt ("GamgExitTimeSecond", (int)gameExitTime.Second);
		Application.Quit ();
	}
	void OnApplicationQuit(){
		PlayerPrefs.SetFloat ("TimeSpend", useSecond);
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
		textUsableColor = PlayerPrefs.GetString ("textUsableColor");
		pathUI = "Sprites/" + theme + "/UI_" + UIColor + "/UI_" + UIColor;
//		Material ball = GameObject.Find ("Background/Background/Stars_soft").GetComponent<Material> ();
//		if (theme == "starysky")
//			ball.SetColor ("_Color", Color.white);
//		else
//			ball.SetColor ("_Color", Color.white);
		GameObject.Find ("Background/Background").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Background01");
		GameObject.Find ("Background/Container").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/" + theme + "/Image_" + UIColor + "/Image_" + UIColor + "_Container");
		GameObject.Find ("Background/Dividinglineh").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		GameObject.Find ("Background/Dividinglines").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (pathUI + "_Dividingline");
		// Canvas下组件的初始化
		string playerEName = PlayerPrefs.GetString ("PlayerName");
		string playerCName = WWW.UnEscapeURL (playerEName);
		GameObject.Find ("Canvas/NameButton/Text").GetComponent<Text> ().text = (textColor + "     " + playerCName + "</color>");
		GameObject.Find ("Canvas/NameButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec03");
		GameObject.Find ("Canvas/RemainingTimeBut").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec03");
		GameObject.Find ("Canvas/RemainingTimeBut/Text").GetComponent<Text> ().text = (textColor + "  剩余时间" + "</color>");
		remainingTimeText = GameObject.Find ("Canvas/RemainingTimeText").GetComponent<Text> ();
		GameObject.Find ("Canvas/SpeedBonusBut").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec03");
		GameObject.Find ("Canvas/SpeedBonusBut/Text").GetComponent<Text> ().text = (textColor + "  速度加成" + "</color>");
		speedBonusText = GameObject.Find ("Canvas/SpeedBonusText").GetComponent<Text> ();
		speedBonusText.text = (textColor + PlayerPrefs.GetInt ("SpeedBonus") + "%" + "</color>");
		GameObject.Find ("Canvas/Square").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/" + theme + "/Image_" + UIColor + "/Image_" + UIColor + "_Square");
		square = GameObject.Find ("Canvas/Square").GetComponent<RectTransform>();
		square.localPosition = vEnable;

		GameObject.Find ("Canvas/AtlasModelButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonClock");
		GameObject.Find ("Canvas/AtlasModelButton/Text").GetComponent<Text> ().text = (textColor + "        闯关模式" + "</color>");
		GameObject.Find ("Canvas/CreateModelButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonClockNo");
		GameObject.Find ("Canvas/CreateModelButton/Text").GetComponent<Text> ().text = (textColor + "        自由模式" + "</color>");
		GameObject.Find ("Canvas/AtlasButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonChange02");
		GameObject.Find ("Canvas/AtlasButton/Text").GetComponent<Text> ().text = (textColor + "        宝库" + "</color>");
		GameObject.Find ("Canvas/ShopButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonShop");
		GameObject.Find ("Canvas/ShopButton/Text").GetComponent<Text> ().text = (textColor + "        商店" + "</color>");
		GameObject.Find ("Canvas/SkinButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonChange02");
		GameObject.Find ("Canvas/SkinButton/Text").GetComponent<Text> ().text = (textColor + "        装扮" + "</color>");
		GameObject.Find ("Canvas/SetButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonSet");
		GameObject.Find ("Canvas/SetButton/Text").GetComponent<Text> ().text = (textColor + "        设置" + "</color>");
		GameObject.Find ("Canvas/ExitButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonExit");
		GameObject.Find ("Canvas/ExitButton/Text").GetComponent<Text> ().text = (textColor + "       退 出" + "</color>");

//		GameObject.Find ("Canvas/QBNumberText").GetComponent<Text> ().text = (textColor + "巧币数量" + "</color>");
//		qBNumber = GameObject.Find ("Canvas/QBNumber").GetComponent<Text> ();
//		qBNumber.text = (textUsableColor + PlayerPrefs.GetInt ("QBNumber") + "</color>" + textColor + "QB" + "</color>");
		GameObject.Find ("Canvas/MyTangramText").GetComponent<Text> ().text = (textColor + "我的七巧板" + "</color>");

		// Square下组件的初始化
		for (int i = 1; i <= 7; i++) {
			tangramCount [i] = 0;
		}
		tangramCount [1] = PlayerPrefs.GetInt ("TangramCount1");
		tangramCount [2] = PlayerPrefs.GetInt ("TangramCount2");
		tangramCount [3] = PlayerPrefs.GetInt ("TangramCount3");
		tangramCount [4] = PlayerPrefs.GetInt ("TangramCount4");
		tangramCount [5] = PlayerPrefs.GetInt ("TangramCount5");
		tangramCount [6] = PlayerPrefs.GetInt ("TangramCount6");
		tangramCount [7] = PlayerPrefs.GetInt ("TangramCount7");
		tangramCountText1 = GameObject.Find ("Canvas/Square/TangramCount1").GetComponent<Text> ();
		tangramCountText2 = GameObject.Find ("Canvas/Square/TangramCount2").GetComponent<Text> ();
		tangramCountText3 = GameObject.Find ("Canvas/Square/TangramCount3").GetComponent<Text> ();
		tangramCountText4 = GameObject.Find ("Canvas/Square/TangramCount4").GetComponent<Text> ();
		tangramCountText5 = GameObject.Find ("Canvas/Square/TangramCount5").GetComponent<Text> ();
		tangramCountText6 = GameObject.Find ("Canvas/Square/TangramCount6").GetComponent<Text> ();
		tangramCountText7 = GameObject.Find ("Canvas/Square/TangramCount7").GetComponent<Text> ();
		ProduceTangram (0);
//		tangramCountText1.text = (textUsableColor + tangramCount [1] + "</color>");
//		tangramCountText2.text = (textUsableColor + tangramCount [2] + "</color>");
//		tangramCountText3.text = (textUsableColor + tangramCount [3] + "</color>");
//		tangramCountText4.text = (textUsableColor + tangramCount [4] + "</color>");
//		tangramCountText5.text = (textUsableColor + tangramCount [5] + "</color>");
//		tangramCountText6.text = (textUsableColor + tangramCount [6] + "</color>");
//		tangramCountText7.text = (textUsableColor + tangramCount [7] + "</color>");

		// CanvasWindow下组件初始化
		GameObject.Find ("CanvasWindow/ReminderWindow").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_Window01");
		GameObject.Find ("CanvasWindow/ReminderWindow/ReminderText").GetComponent<Text> ().text = (textColor + "提示" + "</color>");
		GameObject.Find ("CanvasWindow/ReminderWindow/TipsText").GetComponent<Text> ().text = (textColor + "七巧板不够哦" + "</color>");
		GameObject.Find ("CanvasWindow/ReminderWindow/TureButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		GameObject.Find ("CanvasWindow/ReminderWindow/TureButton/Text").GetComponent<Text> ().text = (textColor + "确定" + "</color>");
		GameObject.Find ("CanvasWindow/ReminderWindow/ReturnButton").GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathUI + "_ButtonRec01");
		GameObject.Find ("CanvasWindow/ReminderWindow/ReturnButton/Text").GetComponent<Text> ().text = (textColor + "返回" + "</color>");
	}
}