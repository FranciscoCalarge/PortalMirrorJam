using System.Collections.Generic;
using UnityEngine;

public class GameData 
{
    [Header("Player Data")]
    public Vector3 playerCurrentPosition;

    [Space(10)]

    [Header("Scene Data")]
    public string currentSceneName;
    public List<bool> wasInteractableActive;
}
