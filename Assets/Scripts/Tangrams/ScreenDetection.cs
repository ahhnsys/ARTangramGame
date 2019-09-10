using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDetection : MonoBehaviour {
	
	private bool isEnter = false;
//	private float triggerTime = 0;
//	private float triggerIntervalTime = 0;
	// 定义委托
	public delegate void ChangeTangramEventHandler (GameObject t);
	static public event ChangeTangramEventHandler ChangeTangram;
	// 判断七巧板状态
//	private bool isTangramMake = false;

	void OnTriggerEnter2D (Collider2D t) {
		// 判断物体是否碰撞触发器
		if (t.GetComponent<Collider2D> ().CompareTag ("Tangram")) {
			isEnter = true;
			Debug.Log("碰撞器进入触发-" + t.gameObject.name);
		}
	}

	void OnTriggerExit2D (Collider2D t) {
		// 如果穿过触发器，则进入或离开编辑区域，执行改变大小操作
		if (isEnter && t.GetComponent<Collider2D> ().CompareTag ("Tangram")) {
			isEnter = false;
			if (ChangeTangram != null)
				ChangeTangram (t.gameObject);
			Debug.Log("碰撞器离开触发-" + t.gameObject.name);
		}
	}
}
//	void OnTriggerEnter2D (Collider2D t) {
//		//		t.gameObject.GetComponent<SpriteRenderer> ().color = Color.black;
//		Debug.Log("触发器触发-" + t.gameObject.name);
//	}
//
//	void OnCollisionEnter2D (Collision2D t) {
//		//		t.gameObject.GetComponent<SpriteRenderer> ().color = Color.black;
//		Debug.Log("碰撞器触发-" + t.gameObject.name);
//	}

