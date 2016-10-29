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

        public DirComponent dir { get { return (DirComponent)GetComponent(CoreComponentIds.Dir); } }
        public bool hasDir { get { return HasComponent(CoreComponentIds.Dir); } }

        public Entity AddDir(UnityEngine.Vector2 newValue) {
            var component = CreateComponent<DirComponent>(CoreComponentIds.Dir);
            component.value = newValue;
            return AddComponent(CoreComponentIds.Dir, component);
        }

        public Entity ReplaceDir(UnityEngine.Vector2 newValue) {
            var component = CreateComponent<DirComponent>(CoreComponentIds.Dir);
            component.value = newValue;
            ReplaceComponent(CoreComponentIds.Dir, component);
            return this;
        }

        public Entity RemoveDir() {
            return RemoveComponent(CoreComponentIds.Dir);
        }
    }
}

    public partial class CoreMatcher {

        static IMatcher _matcherDir;

        public static IMatcher Dir {
            get {
                if(_matcherDir == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.Dir);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherDir = matcher;
                }

                return _matcherDir;
            }
        }
    }
