using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputController
{
    public CharacterBase characterBase { get; set; }

    void SetAnim(PlayerAnimEventArg animEventArg);
    void DoAttack();
    void DoRoll();

}
