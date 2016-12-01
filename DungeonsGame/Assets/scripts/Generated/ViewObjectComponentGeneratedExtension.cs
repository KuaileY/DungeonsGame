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

        public ViewObjectComponent viewObject { get { return (ViewObjectComponent)GetComponent(CoreComponentIds.ViewObject); } }
        public bool hasViewObject { get { return HasComponent(CoreComponentIds.ViewObject); } }

        public Entity AddViewObject(string newName) {
            var component = CreateComponent<ViewObjectComponent>(CoreComponentIds.ViewObject);
            component.name = newName;
            return AddComponent(CoreComponentIds.ViewObject, component);
        }

        public Entity ReplaceViewObject(string newName) {
            var component = CreateComponent<ViewObjectComponent>(CoreComponentIds.ViewObject);
            component.name = newName;
            ReplaceComponent(CoreComponentIds.ViewObject, component);
            return this;
        }

        public Entity RemoveViewObject() {
            return RemoveComponent(CoreComponentIds.ViewObject);
        }
    }
}

    public partial class CoreMatcher {

        static IMatcher _matcherViewObject;

        public static IMatcher ViewObject {
            get {
                if(_matcherViewObject == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.ViewObject);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherViewObject = matcher;
                }

                return _matcherViewObject;
            }
        }
    }
