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

        public DungeonItemsCacheComponent dungeonItemsCache { get { return (DungeonItemsCacheComponent)GetComponent(CoreComponentIds.DungeonItemsCache); } }
        public bool hasDungeonItemsCache { get { return HasComponent(CoreComponentIds.DungeonItemsCache); } }

        public Entity AddDungeonItemsCache(System.Collections.Generic.List<Entitas.Entity[,]> newRoomList) {
            var component = CreateComponent<DungeonItemsCacheComponent>(CoreComponentIds.DungeonItemsCache);
            component.roomList = newRoomList;
            return AddComponent(CoreComponentIds.DungeonItemsCache, component);
        }

        public Entity ReplaceDungeonItemsCache(System.Collections.Generic.List<Entitas.Entity[,]> newRoomList) {
            var component = CreateComponent<DungeonItemsCacheComponent>(CoreComponentIds.DungeonItemsCache);
            component.roomList = newRoomList;
            ReplaceComponent(CoreComponentIds.DungeonItemsCache, component);
            return this;
        }

        public Entity RemoveDungeonItemsCache() {
            return RemoveComponent(CoreComponentIds.DungeonItemsCache);
        }
    }

    public partial class Pool {

        public Entity dungeonItemsCacheEntity { get { return GetGroup(CoreMatcher.DungeonItemsCache).GetSingleEntity(); } }
        public DungeonItemsCacheComponent dungeonItemsCache { get { return dungeonItemsCacheEntity.dungeonItemsCache; } }
        public bool hasDungeonItemsCache { get { return dungeonItemsCacheEntity != null; } }

        public Entity SetDungeonItemsCache(System.Collections.Generic.List<Entitas.Entity[,]> newRoomList) {
            if(hasDungeonItemsCache) {
                throw new EntitasException("Could not set dungeonItemsCache!\n" + this + " already has an entity with DungeonItemsCacheComponent!",
                    "You should check if the pool already has a dungeonItemsCacheEntity before setting it or use pool.ReplaceDungeonItemsCache().");
            }
            var entity = CreateEntity();
            entity.AddDungeonItemsCache(newRoomList);
            return entity;
        }

        public Entity ReplaceDungeonItemsCache(System.Collections.Generic.List<Entitas.Entity[,]> newRoomList) {
            var entity = dungeonItemsCacheEntity;
            if(entity == null) {
                entity = SetDungeonItemsCache(newRoomList);
            } else {
                entity.ReplaceDungeonItemsCache(newRoomList);
            }

            return entity;
        }

        public void RemoveDungeonItemsCache() {
            DestroyEntity(dungeonItemsCacheEntity);
        }
    }
}

    public partial class CoreMatcher {

        static IMatcher _matcherDungeonItemsCache;

        public static IMatcher DungeonItemsCache {
            get {
                if(_matcherDungeonItemsCache == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.DungeonItemsCache);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherDungeonItemsCache = matcher;
                }

                return _matcherDungeonItemsCache;
            }
        }
    }