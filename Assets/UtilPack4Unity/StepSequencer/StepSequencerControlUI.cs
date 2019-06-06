using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UtilPack4Unity;

public class StepSequencerControlUI : MonoBehaviour
{
    [SerializeField]
    InputField column, row, duration;
    [SerializeField]
    StepSequencer stepSequencer;
    [SerializeField]
    Text text;

    private void Awake()
    {
        stepSequencer.AttackEvent += StepSequencer_AttackEvent;
    }

    private void StepSequencer_AttackEvent(int column, int[] activeElementIds)
    {
        var attacked = string.Empty;
        for (var i = 0; i < activeElementIds.Length; i++)
        {
            attacked += activeElementIds[i];
            if (i != activeElementIds.Length - 1)
            {
                attacked += ",";
            }
        }
        text.text = "Column : " + column + "\n" +
            "Attack : [\n" + attacked + "\n]";
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float d;
        if (float.TryParse(duration.text, out d))
        {
            if (d > 0f)
            {
                stepSequencer.duration = d;
            }
        }
        
    }

    public void OnPressedResetButton()
    {
        var c = int.Parse(column.text);
        var r = int.Parse(row.text);
        var d = float.Parse(duration.text);
        stepSequencer.Init(c, r, d);
    }

    public void OnPressedRewindButton()
    {
        stepSequencer.Rewind();
    }

    public void OnPressedStopButton()
    {
        stepSequencer.Stop();
    }

    public void OnPressedPauseButton()
    {
        stepSequencer.Pause();
    }

    public void OnPressedPlayButton()
    {
        stepSequencer.Play();
    }
}
