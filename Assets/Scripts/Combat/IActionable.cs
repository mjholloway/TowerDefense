using System;
using System.Collections;

namespace TowerDefense.Combat
{
    public interface IActionable
    {
        public IEnumerator GetIntent();
    }
}
