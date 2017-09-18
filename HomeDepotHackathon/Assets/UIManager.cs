using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public Animator animator;
    // Use this for initialization
    int matrixSize = 10;
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
            animator.SetBool("NeedHelp", false);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (animator.GetBool("Rendering") == false && FindObjectOfType<DisplayTexture>().running) {
                toggleRender(true);
            } else if (animator.GetBool("Rendering") == true) {
                toggleRender(false);
            }
        }
	}
    
    public void toggleIntro(bool activated) {
        animator.SetBool("NeedHelp", activated);
    }

    public void actualRenderToggle() {
        animator.SetBool("Rendering", !animator.GetBool("Rendering"));
    }

    public void toggleRender(bool activated) {
        animator.SetBool("Rendering", true);
    }
}
