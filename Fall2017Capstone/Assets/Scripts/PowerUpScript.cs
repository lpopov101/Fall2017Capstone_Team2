using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpScript : MonoBehaviour
{

    public string levelName;
    public GameObject gatorp;
	public GameObject PowerUpUI;
    //public Text text;
    public ToastScript toaster;
    public Light light;
	public AudioClip pickupAudio;
	public AudioSource gatorpAudioSource;

    private SpriteRenderer sr;
    private DodgeScript dodgeScript;
    private HighJumpScript highJumpScript;
	private StunScript stunScript;
	private AudioSource powerupAudio;


	void Awake() {
		sr = GetComponent<SpriteRenderer>();
	}

    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        dodgeScript = gatorp.GetComponent<DodgeScript>();
        highJumpScript = gatorp.GetComponent<HighJumpScript>();
		stunScript = gatorp.GetComponent<StunScript>();
		powerupAudio = GetComponent<AudioSource> ();
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        /*if (coll.gameObject.CompareTag ("Player") && sr.enabled) {
			toaster.ImageToast ("dash", 5.0f);
			PlayerPrefs.SetInt("Powerup", 1);
			PlayerPrefs.Save();
			dodgeScript.hasDodgeAbility = true;
			sr.enabled = false;
			light.enabled = false;
		}*/

        if (coll.gameObject.CompareTag("Player") && sr.enabled)
        {
            PlayerPrefs.SetInt("Powerup", 1);
            PlayerPrefs.Save();

			if(levelName == "Shell1") {
#if UNITY_ANDROID
                toaster.AltImageToast("mobiledash", 5.0f);
#else
                if (Input.GetJoystickNames().Length > 0)
                {
                    toaster.ImageToast("xbox_dash", 5.0f);
                }
                else
                {
                    toaster.ImageToast("dash_z", 5.0f);
                }
#endif
				PowerUpUI.SendMessage("Update");
				dodgeScript.hasDodgeAbility = true;
				gatorpAudioSource.clip = pickupAudio;
				gatorpAudioSource.Play ();

			} else if(levelName == "Shell2") {
#if UNITY_ANDROID
				toaster.AltImageToast("mobile_doublejump", 5.0f);
#else
                if (Input.GetJoystickNames().Length > 0)
                {
                    toaster.ImageToast("xbox_doublejump", 5.0f);
                }
                else
                {
                    toaster.ImageToast("doublejump", 5.0f);
                }
#endif
				PowerUpUI.SendMessage("Update");
				highJumpScript.hasHighJumpAbility = true;
			} else if(levelName == "Shell3") {
#if UNITY_ANDROID
				toaster.AltImageToast("mobile_stun", 5.0f);
#else
                if (Input.GetJoystickNames().Length > 0)
                {
                    toaster.ImageToast("xbox_stun", 5.0f);
                }
                else
                {
                    toaster.ImageToast("stun", 5.0f);
                }
#endif
				PowerUpUI.SendMessage("Update");
				stunScript.hasStunAbility = true;
			}

            sr.enabled = false;
            light.enabled = false;
			powerupAudio.mute = true;
			gatorp.SendMessage("checkCollected");
        }
    }

    /*IEnumerator DisplayText() {
		text.text = "";
		yield return new WaitForSeconds (5.0f);
		text.text = "";
	}*/
	public bool getSpriteRendererStatus() {
		return sr.enabled;
	}

	public void setSpriteEnabled(bool enb) {
		sr.enabled = enb;
	}
}
