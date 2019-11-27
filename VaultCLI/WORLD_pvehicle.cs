// This file is part of VaultCLI by heyitsleo.
// 
// Created: 11/27/2019 @ 11:52 AM.

using System.Collections.Generic;

namespace VaultCLI
{
    using VaultLib.Core.Data;
    public class WORLD_pvehicle : CollectionWrapperBase
    {
        public WORLD_pvehicle(VLTCollection collection) : base(collection) { }
        public List<VaultLib.Core.Types.Attrib.RefSpec> transmission()
        {
            return GetArray<VaultLib.Core.Types.Attrib.RefSpec>("transmission");
        }
        // unknown type: UpgradeSpecs
        public List<VaultLib.Core.Types.VLTUnknown> TurboSND()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("TurboSND");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown CLASS()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("CLASS");
        }
        // unknown type: Attrib::StringKey
        public List<VaultLib.Core.Types.VLTUnknown> BEHAVIOR_ORDER()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("BEHAVIOR_ORDER");
        }
        // unknown type: EffectLinkageRecord
        public List<VaultLib.Core.Types.VLTUnknown> RiskCar_Tier()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("RiskCar_Tier");
        }
        public List<System.Single> Rating()
        {
            return GetArray<System.Single>("Rating");
        }
        public VaultLib.Core.Types.Attrib.RefSpec aivehicle()
        {
            return GetValue<VaultLib.Core.Types.Attrib.RefSpec>("aivehicle");
        }
        // unknown type: GRace::RaceCarClass
        public VaultLib.Core.Types.VLTUnknown RacingClass()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("RacingClass");
        }
        // unknown type: EffectLinkageRecord
        public List<VaultLib.Core.Types.VLTUnknown> OnBottomOut()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("OnBottomOut");
        }
        public List<VaultLib.Core.Types.Attrib.RefSpec> brakes()
        {
            return GetArray<VaultLib.Core.Types.Attrib.RefSpec>("brakes");
        }
        // unknown type: Csis::Type_car_type
        public VaultLib.Core.Types.VLTUnknown VerbalType()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("VerbalType");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown BEHAVIOR_MECHANIC_SUSPENSION()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("BEHAVIOR_MECHANIC_SUSPENSION");
        }
        public System.Boolean field_0x529B4433()
        {
            return GetValue<System.Boolean>("0x529B4433");
        }
        public List<System.Single> TopSpeed()
        {
            return GetArray<System.Single>("TopSpeed");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown BEHAVIOR_MECHANIC_EFFECTS()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("BEHAVIOR_MECHANIC_EFFECTS");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown EventSequencer()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("EventSequencer");
        }
        public VaultLib.Core.Types.Attrib.RefSpec chopperspecs()
        {
            return GetValue<VaultLib.Core.Types.Attrib.RefSpec>("chopperspecs");
        }
        public System.Boolean RandomOpponent()
        {
            return GetValue<System.Boolean>("RandomOpponent");
        }
        // unknown type: EffectLinkageRecord
        public List<VaultLib.Core.Types.VLTUnknown> OnScrapeWorld()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("OnScrapeWorld");
        }
        public VaultLib.Core.Types.Attrib.RefSpec rigidbodyspecs()
        {
            return GetValue<VaultLib.Core.Types.Attrib.RefSpec>("rigidbodyspecs");
        }
        // unknown type: eDRIVE_BY_TYPE
        public VaultLib.Core.Types.VLTUnknown WooshType()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("WooshType");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown BEHAVIOR_MECHANIC_RESET()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("BEHAVIOR_MECHANIC_RESET");
        }
        public VaultLib.Core.Types.Attrib.RefSpec frontend()
        {
            return GetValue<VaultLib.Core.Types.Attrib.RefSpec>("frontend");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown BEHAVIOR_MECHANIC_DAMAGE()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("BEHAVIOR_MECHANIC_DAMAGE");
        }
        public VaultLib.Core.Types.Attrib.RefSpec SkidInfo()
        {
            return GetValue<VaultLib.Core.Types.Attrib.RefSpec>("SkidInfo");
        }
        // unknown type: EAIBreakerType
        public VaultLib.Core.Types.VLTUnknown BreakerType()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("BreakerType");
        }
        // unknown type: UpgradeSpecs
        public List<VaultLib.Core.Types.VLTUnknown> ShiftSND()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("ShiftSND");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown BEHAVIOR_MECHANIC_RIGIDBODY()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("BEHAVIOR_MECHANIC_RIGIDBODY");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown MODEL()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("MODEL");
        }
        public VaultLib.Core.Types.Attrib.RefSpec TrafficEngine()
        {
            return GetValue<VaultLib.Core.Types.Attrib.RefSpec>("TrafficEngine");
        }
        public VaultLib.Core.Types.Attrib.RefSpec Trailer()
        {
            return GetValue<VaultLib.Core.Types.Attrib.RefSpec>("Trailer");
        }
        public List<VaultLib.Core.Types.Attrib.RefSpec> Audio()
        {
            return GetArray<VaultLib.Core.Types.Attrib.RefSpec>("Audio");
        }
        public System.String CollectionName()
        {
            return GetValue<System.String>("CollectionName");
        }
        public List<System.Single> Acceleration()
        {
            return GetArray<System.Single>("Acceleration");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown BEHAVIOR_MECHANIC_ENGINE()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("BEHAVIOR_MECHANIC_ENGINE");
        }
        public List<System.Byte> HornType()
        {
            return GetArray<System.Byte>("HornType");
        }
        public System.String DefaultPresetRide()
        {
            return GetValue<System.String>("DefaultPresetRide");
        }
        public List<System.Single> HandlingRating()
        {
            return GetArray<System.Single>("HandlingRating");
        }
        public List<VaultLib.Core.Types.Attrib.RefSpec> chassis()
        {
            return GetArray<VaultLib.Core.Types.Attrib.RefSpec>("chassis");
        }
        public List<VaultLib.Core.Types.Attrib.RefSpec> nos()
        {
            return GetArray<VaultLib.Core.Types.Attrib.RefSpec>("nos");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown BEHAVIOR_MECHANIC_DRAW()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("BEHAVIOR_MECHANIC_DRAW");
        }
        public List<VaultLib.Core.Types.Attrib.RefSpec> tires()
        {
            return GetArray<VaultLib.Core.Types.Attrib.RefSpec>("tires");
        }
        public VaultLib.Core.Types.Attrib.RefSpec damagespecs()
        {
            return GetValue<VaultLib.Core.Types.Attrib.RefSpec>("damagespecs");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown BEHAVIOR_MECHANIC_INPUT()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("BEHAVIOR_MECHANIC_INPUT");
        }
        public VaultLib.Core.Types.Attrib.RefSpec field_0xC8F8F0D3()
        {
            return GetValue<VaultLib.Core.Types.Attrib.RefSpec>("0xC8F8F0D3");
        }
        public List<VaultLib.Core.Types.Attrib.RefSpec> induction()
        {
            return GetArray<VaultLib.Core.Types.Attrib.RefSpec>("induction");
        }
        public System.Single UpgradeLevel()
        {
            return GetValue<System.Single>("UpgradeLevel");
        }
        public List<System.Single> Handling()
        {
            return GetArray<System.Single>("Handling");
        }
        public VaultLib.Core.Types.Attrib.RefSpec OnTireBlow()
        {
            return GetValue<VaultLib.Core.Types.Attrib.RefSpec>("OnTireBlow");
        }
        // unknown type: EffectLinkageRecord
        public List<VaultLib.Core.Types.VLTUnknown> OnHitGround()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("OnHitGround");
        }
        // unknown type: EffectLinkageRecord
        public List<VaultLib.Core.Types.VLTUnknown> HitchHeat()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("HitchHeat");
        }
        // unknown type: EffectLinkageRecord
        public List<VaultLib.Core.Types.VLTUnknown> OnScrapeGround()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("OnScrapeGround");
        }
        // unknown type: EffectLinkageRecord
        public List<VaultLib.Core.Types.VLTUnknown> OnHitWorld()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("OnHitWorld");
        }
        // unknown type: Sound::TriggeredAudioFeature
        public VaultLib.Core.Types.VLTUnknown field_0xE9C61840()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("0xE9C61840");
        }
        public System.Boolean field_0xEACB7696()
        {
            return GetValue<System.Boolean>("0xEACB7696");
        }
        public System.Boolean PlayerUsable()
        {
            return GetValue<System.Boolean>("PlayerUsable");
        }
        public List<VaultLib.Core.Types.Attrib.RefSpec> engine()
        {
            return GetArray<VaultLib.Core.Types.Attrib.RefSpec>("engine");
        }
        public System.Boolean TunedByProduction()
        {
            return GetValue<System.Boolean>("TunedByProduction");
        }
        public System.Boolean HasCoplights()
        {
            return GetValue<System.Boolean>("HasCoplights");
        }
        // unknown type: Attrib::StringKey
        public VaultLib.Core.Types.VLTUnknown BEHAVIOR_MECHANIC_AUDIO()
        {
            return GetValue<VaultLib.Core.Types.VLTUnknown>("BEHAVIOR_MECHANIC_AUDIO");
        }
        // unknown type: EffectLinkageRecord
        public List<VaultLib.Core.Types.VLTUnknown> OnBottomScrape()
        {
            return GetArray<VaultLib.Core.Types.VLTUnknown>("OnBottomScrape");
        }
    }
}