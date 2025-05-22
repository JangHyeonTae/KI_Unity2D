using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGuage : MonoBehaviour
{
    [SerializeField] Image image;
    
    public void GetGuage(float hp)
    {
        image.fillAmount = hp;
    }
}
