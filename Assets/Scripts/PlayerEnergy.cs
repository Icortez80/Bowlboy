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

    public int AvailableCharges => currentEnergy / hitsPerCharge;
    public bool IsFull => AvailableCharges >= maxCharges;

    //add onto the charge meter with a normal shot
    public void AddNormalHit()
    {
        currentEnergy += 1;
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
            return SpecialType.Super;
            //regular EX shot
        }else if (AvailableCharges >= 1)
        {
            currentEnergy -= hitsPerCharge;
            return SpecialType.EX;
        }
        //no special shot available
        return SpecialType.None;
    }
}
