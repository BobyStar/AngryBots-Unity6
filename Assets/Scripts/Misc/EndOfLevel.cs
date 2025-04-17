using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
[UnityEngine.RequireComponent(typeof(BoxCollider))]
public partial class EndOfLevel : MonoBehaviour
{
    public float timeToTriggerLevelEnd;
    public string endSceneName;
    public virtual IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.StartCoroutine(this.FadeOutAudio());
            PlayerMoveController playerMove = other.gameObject.GetComponent<PlayerMoveController>();
            playerMove.enabled = false;
            yield return null;
            float timeWaited = 0f;
            FreeMovementMotor playerMotor = other.gameObject.GetComponent<FreeMovementMotor>();
            while (playerMotor.walkingSpeed > 0f)
            {
                playerMotor.walkingSpeed = playerMotor.walkingSpeed - (Time.deltaTime * 6f);
                if (playerMotor.walkingSpeed < 0f)
                {
                    playerMotor.walkingSpeed = 0f;
                }
                timeWaited = timeWaited + Time.deltaTime;
                yield return null;
            }
            playerMotor.walkingSpeed = 0f;
            yield return new WaitForSeconds(Mathf.Clamp(this.timeToTriggerLevelEnd - timeWaited, 0f, this.timeToTriggerLevelEnd));
            Camera.main.gameObject.SendMessage("WhiteOut");
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(endSceneName);
        }
    }

    public virtual IEnumerator FadeOutAudio()
    {
        AudioListener al = Camera.main.gameObject.GetComponent<AudioListener>();
        if (al)
        {
            while (AudioListener.volume > 0f)
            {
                AudioListener.volume = AudioListener.volume - (Time.deltaTime / this.timeToTriggerLevelEnd);
                yield return null;
            }
        }
    }

    public EndOfLevel()
    {
        this.timeToTriggerLevelEnd = 2f;
        this.endSceneName = "3-4_Pain";
    }

}