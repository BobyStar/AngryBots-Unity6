using UnityEngine;
using System.Collections;

[System.Serializable]
public class ReceiverItem : object
{
    public GameObject receiver;
    public string action;
    public float delay;
    public virtual IEnumerator SendWithDelay(MonoBehaviour sender)
    {
        yield return new WaitForSeconds(this.delay);
        if (this.receiver)
        {
            this.receiver.SendMessage(this.action);
        }
        else
        {
            Debug.LogWarning(((((("No receiver of signal \"" + this.action) + "\" on object ") + sender.name) + " (") + sender.GetType().Name) + ")", sender);
        }
    }

    public ReceiverItem()
    {
        this.action = "OnSignal";
    }

}
[System.Serializable]
public class SignalSender : object
{
    public bool onlyOnce;
    public ReceiverItem[] receivers;
    private bool hasFired;
    public virtual void SendSignals(MonoBehaviour sender)
    {
        if ((this.hasFired == false) || (this.onlyOnce == false))
        {
            int i = 0;
            while (i < this.receivers.Length)
            {
                sender.StartCoroutine(this.receivers[i].SendWithDelay(sender));
                i++;
            }
            this.hasFired = true;
        }
    }

}