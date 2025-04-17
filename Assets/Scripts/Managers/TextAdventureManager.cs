using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public partial class TextAdventureManager : MonoBehaviour
{
    public Transform player;
    public MoodBox[] playableMoodBoxes;
    public float timePerChar;
    private int currentMoodBox;
    private int textAnimation;
    private float timer;
    private Vector3 camOffset;
    public Canvas textAdventureCanvas;
    public Text contentText;
    public virtual void Start()
    {
        if (!this.player)
        {
            this.player = GameObject.FindWithTag("Player").transform;
        }
    }

    public virtual void OnEnable()
    {
        this.textAnimation = 0;
        this.timer = this.timePerChar;
        this.camOffset = Camera.main.transform.position - this.player.position;
        this.BeamToBox(this.currentMoodBox);
        if (this.player)
        {
            PlayerMoveController ctrler = this.player.GetComponent<PlayerMoveController>();
            ctrler.enabled = false;
        }
        
        textAdventureCanvas.gameObject.SetActive(true);
    }

    public virtual void OnDisable()
    {
        // and back to normal player control
        if (this.player)
        {
            PlayerMoveController ctrler = this.player.GetComponent<PlayerMoveController>();
            ctrler.enabled = true;
        }
        
        textAdventureCanvas.gameObject.SetActive(false);
    }

    public virtual void Update()
    {
        contentText.text = "AngryBots \n \n";
        contentText.text = contentText.text + this.playableMoodBoxes[this.currentMoodBox].data.adventureString.Substring(0, this.textAnimation);
        //Debug.Log(contentText.text);
        if (this.textAnimation >= this.playableMoodBoxes[this.currentMoodBox].data.adventureString.Length)
        {
        }
        else
        {
            this.timer = this.timer - Time.deltaTime;
            if (this.timer <= 0f)
            {
                this.textAnimation++;
                this.timer = this.timePerChar;
            }
        }
        this.CheckInput();
    }

    public virtual void BeamToBox(int index)
    {
        if (index > this.playableMoodBoxes.Length)
        {
            return;
        }
        this.player.position = this.playableMoodBoxes[index].transform.position;
        Camera.main.transform.position = this.player.position + this.camOffset;
        this.textAnimation = 0;
        this.timer = this.timePerChar;
    }

    public virtual void CheckInput()
    {
        int input = 0;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            input = 1;
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                input = -1;
            }
        }
        if (input != 0)
        {
            if (this.textAnimation < this.playableMoodBoxes[this.currentMoodBox].data.adventureString.Length)
            {
                this.textAnimation = this.playableMoodBoxes[this.currentMoodBox].data.adventureString.Length;
                input = 0;
            }
        }
        if (input != 0)
        {
            if (((this.currentMoodBox - this.playableMoodBoxes.Length) == -1) && (input < 0))
            {
                input = 0;
            }
            if ((this.currentMoodBox == 0) && (input < 0))
            {
                input = 0;
            }
            if (input != 0)
            {
                this.currentMoodBox = (input + this.currentMoodBox) % this.playableMoodBoxes.Length;
                this.BeamToBox(this.currentMoodBox);
            }
        }
    }

    public TextAdventureManager()
    {
        this.timePerChar = 0.125f;
        this.camOffset = Vector3.zero;
    }

}