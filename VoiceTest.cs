using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

[RequireComponent(typeof(VoiceController))]
public class VoiceTest : MonoBehaviour {

    public Text uiText;

    VoiceController voiceController;

    public void GetSpeech() {
        voiceController.GetSpeech();
    }

    void Start() {
        voiceController = GetComponent<VoiceController>();
    }

    void OnEnable() {
        VoiceController.resultRecieved += OnVoiceResult;
    }

    void OnDisable() {
        VoiceController.resultRecieved -= OnVoiceResult;
    }

    void OnVoiceResult(string text) {
        uiText.text = text;
    }
    void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        OnDisable();
        StartCoroutine(LocalManager.instance.SubmitRequestToLocal(text, Start));
        Debug.Log("Dictation: " + text);
        
    }

    void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.Log("Dictation exception: " + error);
    }
}
