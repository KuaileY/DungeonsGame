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

        public ViewObjectPoolComponent viewObjectPool { get { return (ViewObjectPoolComponent)GetComponent(BoardComponentIds.ViewObjectPool); } }
        public bool hasViewObjectPool { get { return HasComponent(BoardComponentIds.ViewObjectPool); } }

        public Entity AddViewObjectPool(Entitas.ObjectPool<UnityEngine.GameObject> newPool) {
            var component = CreateComponent<ViewObjectPoolComponent>(BoardComponentIds.ViewObjectPool);
            component.pool = newPool;
            return AddComponent(BoardComponentIds.ViewObjectPool, component);
        }

        public Entity ReplaceViewObjectPool(Entitas.ObjectPool<UnityEngine.GameObject> newPool) {
            var component = CreateComponent<ViewObjectPoolComponent>(BoardComponentIds.ViewObjectPool);
            component.pool = newPool;
            ReplaceComponent(BoardComponentIds.ViewObjectPool, component);
            return this;
        }

        public Entity RemoveViewObjectPool() {
            return RemoveComponent(BoardComponentIds.ViewObjectPool);
        }
    }
}

    public partial class BoardMatcher {

        static IMatcher _matcherViewObjectPool;

        public static IMatcher ViewObjectPool {
            get {
                if(_matcherViewObjectPool == null) {
                    var matcher = (Matcher)Matcher.AllOf(BoardComponentIds.ViewObjectPool);
                    matcher.componentNames = BoardComponentIds.componentNames;
                    _matcherViewObjectPool = matcher;
                }

                return _matcherViewObjectPool;
            }
        }
    }

    public partial class CoreMatcher {

        static IMatcher _matcherViewObjectPool;

        public static IMatcher ViewObjectPool {
            get {
                if(_matcherViewObjectPool == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.ViewObjectPool);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherViewObjectPool = matcher;
                }

                return _matcherViewObjectPool;
            }
        }
    }