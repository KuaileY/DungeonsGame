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

        public SpriteListComponent spriteList { get { return (SpriteListComponent)GetComponent(InputComponentIds.SpriteList); } }
        public bool hasSpriteList { get { return HasComponent(InputComponentIds.SpriteList); } }

        public Entity AddSpriteList(System.Collections.Generic.Dictionary<string, UnityEngine.Sprite[]> newSprites) {
            var component = CreateComponent<SpriteListComponent>(InputComponentIds.SpriteList);
            component.sprites = newSprites;
            return AddComponent(InputComponentIds.SpriteList, component);
        }

        public Entity ReplaceSpriteList(System.Collections.Generic.Dictionary<string, UnityEngine.Sprite[]> newSprites) {
            var component = CreateComponent<SpriteListComponent>(InputComponentIds.SpriteList);
            component.sprites = newSprites;
            ReplaceComponent(InputComponentIds.SpriteList, component);
            return this;
        }

        public Entity RemoveSpriteList() {
            return RemoveComponent(InputComponentIds.SpriteList);
        }
    }

    public partial class Pool {

        public Entity spriteListEntity { get { return GetGroup(InputMatcher.SpriteList).GetSingleEntity(); } }
        public SpriteListComponent spriteList { get { return spriteListEntity.spriteList; } }
        public bool hasSpriteList { get { return spriteListEntity != null; } }

        public Entity SetSpriteList(System.Collections.Generic.Dictionary<string, UnityEngine.Sprite[]> newSprites) {
            if(hasSpriteList) {
                throw new EntitasException("Could not set spriteList!\n" + this + " already has an entity with SpriteListComponent!",
                    "You should check if the pool already has a spriteListEntity before setting it or use pool.ReplaceSpriteList().");
            }
            var entity = CreateEntity();
            entity.AddSpriteList(newSprites);
            return entity;
        }

        public Entity ReplaceSpriteList(System.Collections.Generic.Dictionary<string, UnityEngine.Sprite[]> newSprites) {
            var entity = spriteListEntity;
            if(entity == null) {
                entity = SetSpriteList(newSprites);
            } else {
                entity.ReplaceSpriteList(newSprites);
            }

            return entity;
        }

        public void RemoveSpriteList() {
            DestroyEntity(spriteListEntity);
        }
    }
}

    public partial class InputMatcher {

        static IMatcher _matcherSpriteList;

        public static IMatcher SpriteList {
            get {
                if(_matcherSpriteList == null) {
                    var matcher = (Matcher)Matcher.AllOf(InputComponentIds.SpriteList);
                    matcher.componentNames = InputComponentIds.componentNames;
                    _matcherSpriteList = matcher;
                }

                return _matcherSpriteList;
            }
        }
    }