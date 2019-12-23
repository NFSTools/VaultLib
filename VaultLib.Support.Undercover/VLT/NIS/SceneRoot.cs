// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:43 AM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.NIS
{
    [VLTTypeInfo("NIS::SceneRoot")]
    public class SceneRoot : VLTBaseType, IReferencesStrings
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
        public string MarkerName { get; set; }

        private Text _markerNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            SceneRootType = br.ReadEnum<eSceneRoot>();
            _markerNameText = new Text(Class, Field, Collection);
            _markerNameText.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(SceneRootType);
            _markerNameText.Write(vault, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _markerNameText.ReadPointerData(vault, br);
            MarkerName = _markerNameText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _markerNameText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _markerNameText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _markerNameText.GetStrings();
        }

        public SceneRoot(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public SceneRoot(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}