using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : Singleton<EquipManager>
{
    public EquipManager()
    {

    }

    public void PlayerEquipArmor(Equip equip, bool playSound = true)
    {
        User.Instance.UserCharacter.characterBase.EquipArmor(equip);
         
        UImainCanvas.Instance.UIMenu.equip.SetEquipItem(equip);
        if(playSound ) 
            SoundManager.Instance?.PlaySound(SoundDataDefine.EquipArmor);
    }
    public void PlayerUnEquipArmor(ArmorType type, bool playsound = true)
    {
        User.Instance.UserCharacter.characterBase.UnEquipArmor(type);

        UImainCanvas.Instance.UIMenu.equip.SetUIEquipItemIcon((int)type);
        UImainCanvas.Instance.UIMenu.SetUIEquipIcon((int)type);

        if(playsound)
            SoundManager.Instance?.PlaySound(SoundDataDefine.UnEquipArmor);  

    }
}
