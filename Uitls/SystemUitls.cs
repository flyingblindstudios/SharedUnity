using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
    public class SystemUitls : MonoBehaviour
    {
        static public string GenerateID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}