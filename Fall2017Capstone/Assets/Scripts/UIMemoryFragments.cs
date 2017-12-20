using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMemoryFragments : MonoBehaviour {

	public GameObject fragmentsImage;
	public GameObject PowerUp1;
	public GameObject PowerUp2;
	public GameObject PowerUp3;
	public GameObject gatorp;

	private Animator animator;
	private DodgeScript dodgeScript;
	private HighJumpScript highJumpScript;
	private StunScript stunScript;

	void Awake() {
		animator = fragmentsImage.GetComponent<Animator>();

		dodgeScript = gatorp.GetComponent<DodgeScript>();
		highJumpScript = gatorp.GetComponent<HighJumpScript>();
		stunScript = gatorp.GetComponent<StunScript>();
	}

	void Start () {
		ShowPowerUps();
	}

	public void UpdateUIFragments(int memoryCount) {
		fragmentsImage.SetActive(true);
		animator.SetInteger("shardCount", memoryCount);
	}

	public void HideUIFragments() {
		fragmentsImage.SetActive(false);
	}

	public void HidePowerUps() {
		PowerUp1.SetActive(false);
		PowerUp2.SetActive(false);
		PowerUp3.SetActive(false);
	}

	public void ShowPowerUps() {
		Scene scene = SceneManager.GetActiveScene();
		if (scene.name.Equals ("NewShell1")) {
			PowerUp1.SetActive (true);
			if (dodgeScript.hasDodgeAbility == true) {
				Update ();
			} else {
				PowerUp1.GetComponent<Image>().color = new Color (1f, 1f, 1f, .2f);
			}
		} else if (scene.name.Equals ("Shell2")) {
			PowerUp1.SetActive (true);
			PowerUp2.SetActive (true);
			if (highJumpScript.hasHighJumpAbility == true) {
				Update ();
			} else {
				PowerUp2.GetComponent<Image>().color = new Color (1f, 1f, 1f, .2f);
			}
		} else if (scene.name.Equals ("Shell3")) {
			PowerUp1.SetActive (true);
			PowerUp2.SetActive (true);
			PowerUp3.SetActive (true);
			if (stunScript.hasStunAbility == true) {
				Update ();
			} else {
				PowerUp3.GetComponent<Image> ().color = new Color (1f, 1f, 1f, .2f);
			}
		}
	}

	//called from the script PowerUpScript via sendmessage
	void Update() {
		Scene scene = SceneManager.GetActiveScene();
		if (scene.name.Equals ("NewShell1") && dodgeScript.hasDodgeAbility == true) {
			PowerUp1.GetComponent<Image>().color = new Color (1f, 1f, 1f, 1f);
		} else if (scene.name.Equals ("Shell2") && highJumpScript.hasHighJumpAbility == true) {
			PowerUp2.GetComponent<Image>().color = new Color (1f, 1f, 1f, 1f);
		} else if (scene.name.Equals ("Shell3") && stunScript.hasStunAbility == true) {
			PowerUp3.GetComponent<Image>().color = new Color (1f, 1f, 1f, 1f);
		}
	}

}
