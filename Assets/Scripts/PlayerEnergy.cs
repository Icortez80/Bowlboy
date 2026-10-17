using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public enum SpecialType
    {
        None,
        EX,
        Super
    }

    [SerializeField] private int hitsPerCharge = 10;
    [SerializeField] private int maxCharges = 5;
    [SerializeField] private int currentEnergy = 0;
    [SerializeField] private SpriteRenderer[] renderArr;
    [SerializeField] private Material partialStatus;
    [SerializeField] private Material fullStatus;

    public int AvailableCharges => currentEnergy / hitsPerCharge;
    public bool IsFull => AvailableCharges >= maxCharges;
    private Material currentMaterial;

    public void Awake()
    {
        foreach (SpriteRenderer marker in renderArr){
            marker.enabled = false;
            marker.material = partialStatus;
        }
    }

    //add onto the charge meter with a normal shot
    public void AddNormalHit()
    {
        currentEnergy += 1;
        updateUI();
        if(currentEnergy > hitsPerCharge * maxCharges)
        {
            currentEnergy = hitsPerCharge * maxCharges;
        }
    }

    public SpecialType TrySpendSpecial()
    {
        //try for super first
        if(AvailableCharges == maxCharges)
        {
            currentEnergy = 0;
            updateUI();
            return SpecialType.Super;

        //regular EX shot
        }else if (AvailableCharges >= 1)
        {
            currentEnergy -= hitsPerCharge;
            updateUI();
            return SpecialType.EX;
        }

        //no special shot available
        return SpecialType.None;
    }

    private void updateUI()
    {
        if(AvailableCharges == maxCharges)
        {
            currentMaterial = fullStatus;
        }
        else
        {
            currentMaterial = partialStatus;
        }

        for(int i = 0; i < maxCharges; i++)
        {
            if(i < AvailableCharges)
            {
                renderArr[i].enabled = true;
                renderArr[i].material = currentMaterial;
            }
            else
            {
                renderArr[i].enabled = false;
            }
        }
    }
}
