using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Keypad : Interactable
{
    public string correctCode;

    Camera playerCam;
    TMPro.TextMeshPro textEntry;
    Transform closeToKeypadCamera;

    PlayerController player;
    Vector3 playerStartPosition;

    float moveTimer;
    public AnimationCurve cameraMove;

    AudioSource source;
    public AudioClip buttonPress;
    public AudioClip correct;
    public AudioClip incorrect;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        playerCam = Camera.main;
        foreach(Transform child in transform)
        {
            if (child.name == "Screen (TMP)") textEntry = child.GetComponent<TMPro.TextMeshPro>();
            else if (child.name == "Camera Pos") closeToKeypadCamera = child;
        }

        source = GetComponent<AudioSource>();
    }

    override
    public void LookingAt()
    {

    }

    override
    public void Interact()
    {
        playerStartPosition = player.transform.position;
        player.ToggleLook();
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
        player.ToggleLook();
        canInteract = true;
    }

    IEnumerator AcceptInput()
    {
        while(!canInteract)
        {
            string num = "";

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) num = "1";
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) num = "2";
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) num = "3";
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) num = "4";
            else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) num = "5";
            else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) num = "6";
            else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) num = "7";
            else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) num = "8";
            else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) num = "9";
            else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) num = "0";

            if(num != "")
            {
                source.clip = buttonPress;
                source.Play();
            }

            if (textEntry.text.Length < 4) textEntry.text += num;

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                source.clip = buttonPress;
                source.Play();
                if(textEntry.text.Length > 0) textEntry.text = textEntry.text.Substring(0, textEntry.text.Length - 1);
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                if (textEntry.text == correctCode)
                {
                    source.clip = correct;
                    source.Play();
                    activated?.Invoke();
                    break;
                }
                else
                {
                    source.clip = incorrect;
                    source.Play();
                }
            }
            yield return null;
        }
    }
}
