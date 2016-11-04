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

        public PositionComponent position { get { return (PositionComponent)GetComponent(BoardComponentIds.Position); } }
        public bool hasPosition { get { return HasComponent(BoardComponentIds.Position); } }

        public Entity AddPosition(int newGrid, int newRoomId, UnityEngine.Vector3 newValue) {
            var component = CreateComponent<PositionComponent>(BoardComponentIds.Position);
            component.grid = newGrid;
            component.roomId = newRoomId;
            component.value = newValue;
            return AddComponent(BoardComponentIds.Position, component);
        }

        public Entity ReplacePosition(int newGrid, int newRoomId, UnityEngine.Vector3 newValue) {
            var component = CreateComponent<PositionComponent>(BoardComponentIds.Position);
            component.grid = newGrid;
            component.roomId = newRoomId;
            component.value = newValue;
            ReplaceComponent(BoardComponentIds.Position, component);
            return this;
        }

        public Entity RemovePosition() {
            return RemoveComponent(BoardComponentIds.Position);
        }
    }
}

    public partial class BoardMatcher {

        static IMatcher _matcherPosition;

        public static IMatcher Position {
            get {
                if(_matcherPosition == null) {
                    var matcher = (Matcher)Matcher.AllOf(BoardComponentIds.Position);
                    matcher.componentNames = BoardComponentIds.componentNames;
                    _matcherPosition = matcher;
                }

                return _matcherPosition;
            }
        }
    }

    public partial class CoreMatcher {

        static IMatcher _matcherPosition;

        public static IMatcher Position {
            get {
                if(_matcherPosition == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.Position);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherPosition = matcher;
                }

                return _matcherPosition;
            }
        }
    }
