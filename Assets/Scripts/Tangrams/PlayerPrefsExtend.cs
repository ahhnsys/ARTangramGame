using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/*namespace PlayerPrefsExtendType {
	
	public static class PlayerPrefsExtend {
		public static void Save(string objectName,object o){
			Type t = o.GetType ();
			FieldInfo[] fiedls = t.GetFields ();
			for (int i = 0; i < fiedls.Length; i++) {
				string saveName = objectName + "." + fiedls [i].Name;
				switch (fiedls [i].FieldType.Name) {
				case "String":
					CryptoPrefs.SetString (saveName, fiedls [i].Getvalue (0).ToString ());
					break;
				case "Int":
					CryptoPrefs.SetInt (saveName, fiedls [i].Getvalue (0));
					break;
				case "Float":
					CryptoPrefs.SetFloat (saveName, fiedls [i].Getvalue (0));
					break;
				}
			}
		}

		public static T GetValue <T> (string objectName) where T : new() {
			T newObj = new T ();

			Type t = newObj.GetType ();
			FieldInfo[] fiedls = t.GetFields ();
			for (int i = 0; i < fiedls.Length; i++) {
				string saveName = objectName + "." + fiedls [i].Name;
				switch (fiedls [i].FieldType.Name) {
				case "String":
					fiedls [i].SetValue (newObj, CryptoPrefs.GetString (saveName));
					break;
				case "Int":
					fiedls [i].SetValue (newObj, CryptoPrefs.GetInt (saveName));
					break;
				case "Float":
					fiedls [i].SetValue (newObj, CryptoPrefs.GetFloat (saveName));
					break;
				}
			}
			return newObj;
		}
	}
	
}*/
