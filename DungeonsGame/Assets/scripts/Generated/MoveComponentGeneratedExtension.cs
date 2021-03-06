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

        public MoveComponent move { get { return (MoveComponent)GetComponent(CoreComponentIds.Move); } }
        public bool hasMove { get { return HasComponent(CoreComponentIds.Move); } }

        public Entity AddMove(float newDelay) {
            var component = CreateComponent<MoveComponent>(CoreComponentIds.Move);
            component.delay = newDelay;
            return AddComponent(CoreComponentIds.Move, component);
        }

        public Entity ReplaceMove(float newDelay) {
            var component = CreateComponent<MoveComponent>(CoreComponentIds.Move);
            component.delay = newDelay;
            ReplaceComponent(CoreComponentIds.Move, component);
            return this;
        }

        public Entity RemoveMove() {
            return RemoveComponent(CoreComponentIds.Move);
        }
    }
}

    public partial class CoreMatcher {

        static IMatcher _matcherMove;

        public static IMatcher Move {
            get {
                if(_matcherMove == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.Move);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherMove = matcher;
                }

                return _matcherMove;
            }
        }
    }
