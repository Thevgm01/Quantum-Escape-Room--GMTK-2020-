using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Keypad : Interactable
{
    Action codeSuccess = delegate { };
    Action codeFailure = delegate { };

    string correctCode = "057897";

    Camera playerCam;
    TMPro.TextMeshPro textEntry;
    Transform closeToKeypadCamera;

    PlayerController player;
    Vector3 playerStartPosition;

    float moveTimer;
    public AnimationCurve cameraMove;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main;
        foreach(Transform child in transform)
        {
            if (child.name == "Screen (TMP)") textEntry = child.GetComponent<TMPro.TextMeshPro>();
            else if (child.name == "Camera Pos") closeToKeypadCamera = child;
        }
    }

    override
    public void Interact(PlayerController interactor)
    {
        player = interactor;
        playerStartPosition = interactor.transform.position;
        player.ToggleFocus();
        canInteract = false;
        StartCoroutine("ApproachKeypad");
        StartCoroutine("AcceptInput");
    }

    IEnumerator ApproachKeypad()
    {
        while (moveTimer < cameraMove.keys[cameraMove.length - 1].time)
        {
            if (player.transform.position != playerStartPosition) break;
            moveTimer += Time.deltaTime;
            playerCam.transform.position = Vector3.Lerp(playerCam.transform.parent.position, closeToKeypadCamera.position, cameraMove.Evaluate(moveTimer));
            playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.parent.rotation, closeToKeypadCamera.rotation, cameraMove.Evaluate(moveTimer));
            yield return null;
        }
        while(player.transform.position == playerStartPosition)
        {
            yield return null;
        }
        while (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime * 2;
            playerCam.transform.position = Vector3.Lerp(playerCam.transform.parent.position, closeToKeypadCamera.position, cameraMove.Evaluate(moveTimer));
            playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.parent.rotation, closeToKeypadCamera.rotation, cameraMove.Evaluate(moveTimer));
            yield return null;
        }
        player.ToggleFocus();
        canInteract = true;
    }

    IEnumerator AcceptInput()
    {
        while(!canInteract)
        {
            if (textEntry.text.Length < 6)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) textEntry.text += "1";
                else if (Input.GetKeyDown(KeyCode.Alpha2)) textEntry.text += "2";
                else if (Input.GetKeyDown(KeyCode.Alpha3)) textEntry.text += "3";
                else if (Input.GetKeyDown(KeyCode.Alpha4)) textEntry.text += "4";
                else if (Input.GetKeyDown(KeyCode.Alpha5)) textEntry.text += "5";
                else if (Input.GetKeyDown(KeyCode.Alpha6)) textEntry.text += "6";
                else if (Input.GetKeyDown(KeyCode.Alpha7)) textEntry.text += "7";
                else if (Input.GetKeyDown(KeyCode.Alpha8)) textEntry.text += "8";
                else if (Input.GetKeyDown(KeyCode.Alpha9)) textEntry.text += "9";
                else if (Input.GetKeyDown(KeyCode.Alpha0)) textEntry.text += "0";
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) textEntry.text = textEntry.text.Substring(0, textEntry.text.Length - 1);

            yield return null;
        }
        textEntry.text = "";
    }
}
