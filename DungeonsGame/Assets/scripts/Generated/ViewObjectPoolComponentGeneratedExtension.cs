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

        public ViewObjectPoolComponent viewObjectPool { get { return (ViewObjectPoolComponent)GetComponent(InputComponentIds.ViewObjectPool); } }
        public bool hasViewObjectPool { get { return HasComponent(InputComponentIds.ViewObjectPool); } }

        public Entity AddViewObjectPool(Entitas.MyObjectPool<UnityEngine.GameObject> newPool) {
            var component = CreateComponent<ViewObjectPoolComponent>(InputComponentIds.ViewObjectPool);
            component.pool = newPool;
            return AddComponent(InputComponentIds.ViewObjectPool, component);
        }

        public Entity ReplaceViewObjectPool(Entitas.MyObjectPool<UnityEngine.GameObject> newPool) {
            var component = CreateComponent<ViewObjectPoolComponent>(InputComponentIds.ViewObjectPool);
            component.pool = newPool;
            ReplaceComponent(InputComponentIds.ViewObjectPool, component);
            return this;
        }

        public Entity RemoveViewObjectPool() {
            return RemoveComponent(InputComponentIds.ViewObjectPool);
        }
    }

    public partial class Pool {

        public Entity viewObjectPoolEntity { get { return GetGroup(InputMatcher.ViewObjectPool).GetSingleEntity(); } }
        public ViewObjectPoolComponent viewObjectPool { get { return viewObjectPoolEntity.viewObjectPool; } }
        public bool hasViewObjectPool { get { return viewObjectPoolEntity != null; } }

        public Entity SetViewObjectPool(Entitas.MyObjectPool<UnityEngine.GameObject> newPool) {
            if(hasViewObjectPool) {
                throw new EntitasException("Could not set viewObjectPool!\n" + this + " already has an entity with ViewObjectPoolComponent!",
                    "You should check if the pool already has a viewObjectPoolEntity before setting it or use pool.ReplaceViewObjectPool().");
            }
            var entity = CreateEntity();
            entity.AddViewObjectPool(newPool);
            return entity;
        }

        public Entity ReplaceViewObjectPool(Entitas.MyObjectPool<UnityEngine.GameObject> newPool) {
            var entity = viewObjectPoolEntity;
            if(entity == null) {
                entity = SetViewObjectPool(newPool);
            } else {
                entity.ReplaceViewObjectPool(newPool);
            }

            return entity;
        }

        public void RemoveViewObjectPool() {
            DestroyEntity(viewObjectPoolEntity);
        }
    }
}

    public partial class InputMatcher {

        static IMatcher _matcherViewObjectPool;

        public static IMatcher ViewObjectPool {
            get {
                if(_matcherViewObjectPool == null) {
                    var matcher = (Matcher)Matcher.AllOf(InputComponentIds.ViewObjectPool);
                    matcher.componentNames = InputComponentIds.componentNames;
                    _matcherViewObjectPool = matcher;
                }

                return _matcherViewObjectPool;
            }
        }
    }