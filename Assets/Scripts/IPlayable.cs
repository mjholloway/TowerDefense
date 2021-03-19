using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayable
{
    bool targets { get; set; }

    void PlayCard();
}
