using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {
	
	public UI_MainSence UIMS;
	string textColor;
	int day;
	int hour;
	int minute = 0;
	int second = 0;
	int useSecond = 0;
	int timeInterval = 600;
	float speedBonus = 0;
	float timeStep = 1.0f;
	float timeSpend = 0.0f;

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	// 初始化调用
	void Start () {
		UIMS = GameObject.Find ("Canvas").GetComponent<UI_MainSence> ();
		textColor = PlayerPrefs.GetString ("textColor");
		timeInterval = PlayerPrefs.GetInt ("TimeInterval");
		System.DateTime gameStartTime = System.DateTime.Now;
		if (PlayerPrefs.GetInt ("GamgExitTimeDay") != 0) {
			day = (int)gameStartTime.DayOfYear - PlayerPrefs.GetInt ("GamgExitTimeDay");
			hour = (int)gameStartTime.Hour - PlayerPrefs.GetInt ("GamgExitTimeHour");
			minute = (int)gameStartTime.Minute - PlayerPrefs.GetInt ("GamgExitTimeMinute");
			second = (int)gameStartTime.Second - PlayerPrefs.GetInt ("GamgExitTimeSecond");
			if (day >= 0) {
				hour += 24;
				int number = 0;
				number = (hour * 3600 + minute * 60 + second) / 10;
				UIMS.ProduceTangram (number);
			}
		}
		speedBonus = PlayerPrefs.GetInt ("SpeedBonus") / 100.0f;
		timeStep = 1.0f / (speedBonus + 1);

	}
		
	// 每一帧调用

	void Update () {
		timeSpend += Time.deltaTime;
		// 每过一个时间步长，当前累计的时间+1
		if (timeSpend >= timeStep) {
			useSecond++;
			if (useSecond >= timeInterval) {
				UIMS.ProduceTangram (1);
				minute = 10;
				second = 0;
				UIMS.remainingTimeText.text = (textColor + string.Format ("{0:D2}:{1:D2}", minute, second) + "</color>");
				useSecond = 0;
			} else {
				minute = (timeInterval - useSecond) / 60;
				second = timeInterval - useSecond - minute * 60;
				UIMS.remainingTimeText.text = (textColor + string.Format ("{0:D2}:{1:D2}", minute, second) + "</color>");
			}
			timeSpend = 0.0f;
		}

	}
}
