// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 11/03/2019 @ 5:55 PM.

using CoreLibraries.ModuleSystem;

namespace VaultLib.Core.ModuleHelper
{
    public abstract class BaseVLTModule : IDataModule
    {
        protected const string VLTFeatureKey = "VLT";

        public abstract void Load();
    }
}