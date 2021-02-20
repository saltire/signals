using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour {
  public float speed = 4;

  SpriteRenderer spriter;

  Oscillator osc;

  void Start() {
    spriter = GetComponent<SpriteRenderer>();
  }

  void FixedUpdate() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad != null) {
      transform.Translate(gamepad.leftStick.ReadValue() * Time.deltaTime * speed);

      if (osc != null) {
        if (gamepad.leftTrigger.isPressed) {
          osc.AdjustFrequency(gamepad.rightStick.ReadValue().y);
        }
        if (gamepad.rightTrigger.isPressed) {
          osc.AdjustVolume(gamepad.rightStick.ReadValue().y);
        }
      }
    }
  }

  void OnTriggerEnter2D(Collider2D other) {
    spriter.material.color = Color.red;
    other.GetComponent<SpriteRenderer>().material.color = Color.yellow;
    osc = other.GetComponent<Oscillator>();
  }

  void OnTriggerExit2D(Collider2D other) {
    spriter.material.color = Color.white;
    other.GetComponent<SpriteRenderer>().material.color = Color.white;
    osc = null;
  }
}
