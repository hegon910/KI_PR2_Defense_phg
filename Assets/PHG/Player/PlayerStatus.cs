using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    [field: SerializeField][field:Range(0, 10)]
    public float RoatateSpeed { get; set; }

    [field:SerializeField][field:Range(0, 1000)]
    public int MaxHP { get; set; }


}
