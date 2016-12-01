//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Entitas;

namespace Entitas {

    public partial class Entity {

        public ItemTypeComponent itemType { get { return (ItemTypeComponent)GetComponent(CoreComponentIds.ItemType); } }
        public bool hasItemType { get { return HasComponent(CoreComponentIds.ItemType); } }

        public Entity AddItemType(ItemType newType) {
            var component = CreateComponent<ItemTypeComponent>(CoreComponentIds.ItemType);
            component.type = newType;
            return AddComponent(CoreComponentIds.ItemType, component);
        }

        public Entity ReplaceItemType(ItemType newType) {
            var component = CreateComponent<ItemTypeComponent>(CoreComponentIds.ItemType);
            component.type = newType;
            ReplaceComponent(CoreComponentIds.ItemType, component);
            return this;
        }

        public Entity RemoveItemType() {
            return RemoveComponent(CoreComponentIds.ItemType);
        }
    }
}

    public partial class CoreMatcher {

        static IMatcher _matcherItemType;

        public static IMatcher ItemType {
            get {
                if(_matcherItemType == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.ItemType);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherItemType = matcher;
                }

                return _matcherItemType;
            }
        }
    }
