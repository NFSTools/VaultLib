// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:43 AM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.NIS;

[VltTypeInfo("NIS::SceneRoot")]
public class SceneRoot: VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings
{
    public enum eSceneRoot
    {
        ROOT_WORLD_ORIGIN = 0x0,
        ROOT_PLAYER_CAR_POS = 0x1,
        ROOT_PLAYER_CAR_POS_AT_GROUND_HEIGHT = 0x2,
        ROOT_PLAYER_CAR_POS_ORIENTED_TO_GROUND = 0x3,
        ROOT_PLAYER_CAMERA_POS = 0x4,
        ROOT_PLAYER_CAMERA_POS_AT_GROUND_HEIGHT = 0x5,
        ROOT_PLAYER_CAMERA_POS_ORIENTED_TO_GROUND = 0x6,
        ROOT_BIG_BANG_MARKER = 0x7,
        ROOT_BIG_BANG_MARKER_AT_GROUND_HEIGHT = 0x8,
        ROOT_BIG_BANG_MARKER_ORIENTED_TO_GROUND = 0x9,
        ROOT_TRACK_MARKER = 0xA,
        ROOT_TRACK_MARKER_AT_GROUND_HEIGHT = 0xB,
        ROOT_TRACK_MARKER_ORIENTED_TO_GROUND = 0xC,
    }

    public eSceneRoot SceneRootType { get; set; }
    public string MarkerName { get; set; } = string.Empty;

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        SceneRootType = br.ReadEnum<eSceneRoot>();
        MarkerName = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(SceneRootType);
        context.WriteString(MarkerName, fieldContext, bw);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { MarkerName };
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}