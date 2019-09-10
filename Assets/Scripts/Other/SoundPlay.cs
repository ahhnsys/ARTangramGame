using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class SoundPlay : MonoBehaviour {
	// 将准备好的MP3格式的背景声音文件拖入此处
	public AudioClip backgroundMusic;

	// 将准备好的MP3格式的背景声音文件拖入此处
	public AudioClip tangramMusic;

	// 将准备好的MP3格式的音效文件拖入此处
	public AudioClip moveSound;
	 
	// 用于控制声音的AudioSource组件
	private AudioSource backGroundAudio;
	private AudioSource moveAudio;

	// 是否播放游戏背景音乐
	private bool isPlayBackMusic;
	// 是否播放按键音效
	private bool isPlayMoveSound;
	// 是否在设置界面
	private bool isInSettingScene;

	void Awake () {
		DontDestroyOnLoad (this.gameObject);
		SceneManager.LoadScene ("MainSence");

//		string nowScene = SceneManager.GetActiveScene ().name;
//		isInSettingScene = nowScene.Equals ("Setting") ? true : false;
		isPlayBackMusic = PlayerPrefs.GetInt ("IsPlayBackGroundSound") == 0 ? false : true;
		isPlayMoveSound = PlayerPrefs.GetInt ("IsPlayMoveSound") == 0 ? false : true;

		// 在添加此脚本的对象中添加AudioSource组件
		backGroundAudio = this.gameObject.AddComponent<AudioSource>();
		moveAudio = this.gameObject.AddComponent<AudioSource> ();

//		// 设置循环播放 
		backGroundAudio.loop = true;
//		moveAudio.loop = false;

		// 设置音量，区间在0-1之间
		backGroundAudio.volume = PlayerPrefs.GetFloat("PlayBackGroundSound");
		moveAudio.volume = PlayerPrefs.GetFloat ("PlayMoveSound");

		//设置audioClip
		backGroundAudio.clip = backgroundMusic;
//		if (nowScene.Equals ("TangramAssemble")) {
//			backGroundAudio.clip = tangramMusic;
//		}
		moveAudio.clip = moveSound;

		if (isPlayBackMusic) {
			backGroundAudio.Play ();
		}
	}

	void Update () {
		// 如果在设置场景，则要实时反馈音乐、音效和音量设置
		// 如果isPlayBackMusic为false,则暂停播放背景音乐
		if (isInSettingScene) {
			isPlayBackMusic = PlayerPrefs.GetInt ("IsPlayBackGroundSound") == 0 ? false : true;
			backGroundAudio.volume = PlayerPrefs.GetFloat("PlayBackGroundSound");
			if (!isPlayBackMusic)
				backGroundAudio.Pause ();
			else
				PlayAgain ();
		}
	}

	public void InSettingScene () {
		isInSettingScene = true;
	}

	public void InAssembleScene () {
		if (backGroundAudio.clip != tangramMusic) {
			backGroundAudio.clip = tangramMusic;
			backGroundAudio.Play ();
		}
	}

	public void outAssembleScene () {
		backGroundAudio.clip = backgroundMusic;
		backGroundAudio.Play ();
	}

	public void PlayMoveSound () {
		if (isPlayMoveSound == true)
			moveAudio.Play ();
	}

	public void PlayAgain(){
		if (!backGroundAudio.isPlaying)
			backGroundAudio.Play ();
	}
}

/*
void OnGUI()
{
	// 绘制播放按钮并设置其点击后的处理
	if (GUI.Button(new Rect(10, 10, 80, 30), "Play"))
	{
		// 播放声音
		if (isPlayBackMusic)
			audioSource.Play();
		AddBtnSound();
	}
	// 绘制暂停按钮并设置其点击后的处理
	if (GUI.Button(new Rect(10, 50, 80, 30), "Pause"))
	{
		// 暂停声音，暂停后再播放，则声音为继续播放
		audioSource.Pause();
		AddBtnSound();
	}
	// 绘制停止按钮并设置其点击后的处理
	if (GUI.Button(new Rect(10, 90, 80, 30), "Stop"))
	{
		// 停止播放，停止后再播放，则声音为从头播放
		audioSource.Stop();
		AddBtnSound();
	}
	// 绘制添加音效按钮,PlayOnShot方式添加音效
	if (GUI.Button(new Rect(100, 10, 150, 30),"AddSound_Method_1"))
	{
		audioSource.PlayOneShot(moveSound);
		AddBtnSound();
	}
	// 绘制添加音效按钮，PlayClipAtPoint方式添加音效
	if (GUI.Button(new Rect(100, 50, 150, 30),"AddSound_Method_2"))
	{
		AudioSource.PlayClipAtPoint(moveSound,audioSource.transform.localPosition);
		AddBtnSound();
	}
	// 音量控制Label
	GUI.Label(new Rect(10,130,100,30),"音量大小调节");
	// 音量控制slider
	audioSource.volume = GUI.HorizontalSlider(new Rect(120, 130, 100, 20),audioSource.volume, 0.0f, 1.0f);
	// 选择是否播放背景音乐
	isPlayBackMusic = GUI.Toggle(new Rect(10,170,100,20),isPlayBackMusic, "背景音乐");
	// 选择是否播放按键声音
	isPlayMoveSound = GUI.Toggle(new Rect(120,170,100,20),isPlayMoveSound,"按钮音效");
}
// 添加按键声音
private void AddBtnSound()
{
	if (isPlayButtonSound)AudioSource.PlayClipAtPoint(buttonSound,audioSource.transform.localPosition);
}
*/