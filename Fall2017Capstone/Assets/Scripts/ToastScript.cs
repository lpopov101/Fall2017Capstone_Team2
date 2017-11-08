using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastScript : MonoBehaviour {

	Text Toaster;
	public Image ImageUI;
    public Image AltImageUI;

	// Use this for initialization
	void Start () {
		Toaster = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {

	}

	public void ImageToast(string imgpath, float time) {
		Sprite img = Resources.Load<Sprite> (imgpath);
		StartCoroutine(DisplayImage(img, time));
	}
    public void AltImageToast(string imgpath, float time)
    {
        Sprite img = Resources.Load<Sprite>(imgpath);
        StartCoroutine(DisplayAltImage(img, time));
    }

	public void Toast(string text,float time) {
		StartCoroutine(DisplayText(text,time));
	}

	IEnumerator DisplayText(string text, float time) {
		Toaster.text = text;
		Toaster.CrossFadeAlpha(0.0f, time, false);
		yield return new WaitForSeconds(time);
		Toaster.text = "";
		Toaster.CrossFadeAlpha(1.0f, 0.1f, false);
	}

	IEnumerator DisplayImage(Sprite img, float time) {
		Debug.Log (img);
		//Color imageColor = ImageUI.color;
		//imageColor.a = 1.0f;
		//ImageUI.color = imageColor;
		ImageUI.sprite = img;
		ImageUI.CrossFadeAlpha(1.0f, 0.0f, false);
		ImageUI.CrossFadeAlpha(0.0f, time, false);
		yield return new WaitForSeconds(time);
		//Toaster.text = "";
		//ImageUI = null;
	}

    IEnumerator DisplayAltImage(Sprite img, float time)
    {
        Debug.Log(img);
        AltImageUI.sprite = img;
        AltImageUI.CrossFadeAlpha(1.0f, 0.0f, false);
        AltImageUI.CrossFadeAlpha(0.0f, time, false);
        yield return new WaitForSeconds(time);
    }
}