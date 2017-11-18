using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpScript : MonoBehaviour
{

    public string levelName;
    public GameObject gatorp;
    //public Text text;
    public ToastScript toaster;
    public Light light;

    private SpriteRenderer sr;
    private DodgeScript dodgeScript;
    private HighJumpScript highJumpScript;
	private StunScript stunScript;

	void Awake() {
		sr = GetComponent<SpriteRenderer>();
	}

    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        dodgeScript = gatorp.GetComponent<DodgeScript>();
        highJumpScript = gatorp.GetComponent<HighJumpScript>();
		stunScript = gatorp.GetComponent<StunScript>();
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
				toaster.ImageToast("dash_z", 5.0f);
#endif
				dodgeScript.hasDodgeAbility = true;
			} else if(levelName == "Shell2") {
#if UNITY_ANDROID
				//toaster.AltImageToast("mobiledoublejump", 5.0f);
#else
				toaster.ImageToast("doublejump", 5.0f);
#endif
				highJumpScript.hasHighJumpAbility = true;
			} else if(levelName == "Shell3") {
#if UNITY_ANDROID
				//toaster.AltImageToast("mobilestun", 5.0f);
#else
				toaster.ImageToast("stun", 5.0f);
#endif
				stunScript.hasStunAbility = true;
			}

            sr.enabled = false;
            light.enabled = false;
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
