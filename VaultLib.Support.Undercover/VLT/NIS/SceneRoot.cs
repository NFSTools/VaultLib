// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:43 AM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.NIS
{
    [VltTypeInfo("NIS::SceneRoot")]
    public class SceneRoot : VltBaseType, IReferencesStrings
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

        private Text _markerNameText = new();

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            SceneRootType = br.ReadEnum<eSceneRoot>();
            _markerNameText.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.WriteEnum(SceneRootType);
            _markerNameText.Value = MarkerName;
            _markerNameText.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _markerNameText.ReadPointerData(context, fieldContext, br);
            MarkerName = _markerNameText.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _markerNameText.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            _markerNameText.AddPointers(context, fieldContext);
        }

        public IEnumerable<string> GetStrings()
        {
            return _markerNameText.GetStrings();
        }
    }
}