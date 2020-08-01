using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagAttTest : MonoBehaviour
{

    [Tag(isUnityTagField = true)] 
    [SerializeField] private string tagTest; 

}
