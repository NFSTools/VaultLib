﻿using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(EAIBreakerType))]
    public enum EAIBreakerType
    {
        BT_NONE = 0x0,
        BT_REAR = 0x1,
        BT_SIDE = 0x2,
    }
}
