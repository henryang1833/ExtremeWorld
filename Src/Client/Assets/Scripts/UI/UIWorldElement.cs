﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElement : MonoBehaviour
{
    public Transform owner;
    public float height = 2.5f;

    void Update()
    {
        if (owner != null)
            this.transform.position = owner.transform.position + Vector3.up * height;

        if (Camera.main != null)
            this.transform.forward = Camera.main.transform.forward;
    }
}
