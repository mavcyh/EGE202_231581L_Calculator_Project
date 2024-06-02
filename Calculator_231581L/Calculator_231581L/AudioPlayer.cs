using NAudio.Wave;
using System.Collections.Generic;
using System.IO;
using System.Speech.Synthesis;

public class AudioPlayer
{
    readonly List<WaveOutEvent> waveOutEvents = new List<WaveOutEvent>();
    public bool resultsVoiceEnabled = false, clickSoundsEnabled = true;
    SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

    public AudioPlayer()
    {
        speechSynthesizer.Rate = 4;
    }

    public void ToggleClickSounds()
    {
        clickSoundsEnabled = !clickSoundsEnabled;
    }

    public void ToggleResultsVoice()
    {
        resultsVoiceEnabled = !resultsVoiceEnabled;
    }

    public void PlayResource(byte[] resource)
    {
        if (!clickSoundsEnabled) return;
        MemoryStream mp3File = new MemoryStream(resource);
        var waveStream = new Mp3FileReader(mp3File);
        var waveOutEvent = new WaveOutEvent();

        waveOutEvent.Init(waveStream);
        waveOutEvent.PlaybackStopped += (sender, args) =>
        {
            waveStream.Dispose();
            waveOutEvents.Remove(waveOutEvent);
        };
        waveOutEvents.Add(waveOutEvent);
        waveOutEvent.Play();
    }

    public void PlayResult(string displayString)
    {   
        if (!resultsVoiceEnabled) return;
        string[] displayStringSplit = displayString.Split('e'), displayStringDpSplit = displayStringSplit[0].Split('.');
        string text;
        if (displayStringDpSplit[1] != "") // Space apart numbers after DP for reading
        {   
            string spacedDisplayStringDp = "";
            foreach (char digit in displayStringDpSplit[1]) spacedDisplayStringDp = spacedDisplayStringDp + digit + " ";
            displayStringSplit[0] = displayStringDpSplit[0] + "point" + spacedDisplayStringDp;
        }
        else displayStringSplit[0].Substring(0, displayStringSplit[0].Length - 1); // Remove DP
        text = displayStringSplit[0];
        if (displayStringSplit.Length > 1) text = text + "times ten to the power of " + displayStringSplit[1];
        speechSynthesizer.SpeakAsync(text.Replace("-", " negative "));
    }
}
