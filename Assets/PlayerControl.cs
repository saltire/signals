using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
  public float speed = 4;

  SpriteRenderer spriter;

  void Start() {
    spriter = GetComponent<SpriteRenderer>();
  }

  void Update() {
    Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    transform.position += move * Time.deltaTime * speed;
  }

  void OnTriggerEnter2D(Collider2D other) {
    spriter.material.color = Color.red;
    other.GetComponent<SpriteRenderer>().material.color = Color.yellow;
  }

  void OnTriggerExit2D(Collider2D other) {
    spriter.material.color = Color.white;
    other.GetComponent<SpriteRenderer>().material.color = Color.white;
  }
}
