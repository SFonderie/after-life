using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PasswordManager : MonoBehaviour
{
    [SerializeField] TMP_Text interactionUI;

    [SerializeField] GameObject passwordPanel;
    [SerializeField] TMP_Text codeTriedText;
    [SerializeField] int correctCode;

    bool isPlayerColliding = false;

    PlayerCamera playerCamera;
    string code;
    int currentCodeLength = 0;
    const int maxCodeLength = 4;

    void Start(){
        interactionUI.gameObject.SetActive(false);
        playerCamera = FindObjectOfType<PlayerCamera>();
        passwordPanel.SetActive(false);
        codeTriedText.text = "_ _ _ _";
    }

    void Update(){
        if(isPlayerColliding){            
            if (Input.GetKeyDown(KeyCode.E)){
                Debug.Log("Open password UI");
                interactionUI.gameObject.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                passwordPanel.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerManager>()){
            interactionUI.gameObject.SetActive(true);
            isPlayerColliding = true;        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PlayerManager>()){
            interactionUI.gameObject.SetActive(false);
            isPlayerColliding = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
                passwordPanel.SetActive(false);
        }
    }

    public void SetDigitInCode(int digit){
        currentCodeLength++;
        if(currentCodeLength > maxCodeLength){
            currentCodeLength = 1;
            code = digit.ToString();
            codeTriedText.text = code + " _ _ _";
        }
        else{
            code = code + digit.ToString();
            string temp_code = code;
            for(int i = 0; i < maxCodeLength - currentCodeLength; i++){
                temp_code += " _";
            }
            codeTriedText.text = temp_code;
        }
        Debug.Log(code);
    }

    public void CheckCode(){
        if(code == correctCode.ToString()){
            Debug.Log("Open drawer");
        }
        else{
            Debug.Log("Wrong code");
            code = "";
            codeTriedText.text = "_ _ _ _";
        }
    }

}
