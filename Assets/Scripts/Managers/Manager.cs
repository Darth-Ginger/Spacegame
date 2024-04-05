using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// @defgroup managers Managers
/// @summary
///    Manager Classes <summary>

/// <summary>
///     Base class for all manager classes
/// </summary>
/// @ingroup managers
/// @brief Base class for Managers
public abstract class Manager : MonoBehaviour
{
    protected virtual void Awake()      {}
    protected virtual void Update()     {}
    protected virtual void LateUpdate() {}
}