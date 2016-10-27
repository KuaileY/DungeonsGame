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

        public ItemBoardComponent itemBoard { get { return (ItemBoardComponent)GetComponent(CoreComponentIds.ItemBoard); } }
        public bool hasItemBoard { get { return HasComponent(CoreComponentIds.ItemBoard); } }

        public Entity AddItemBoard(SingleGrid newRoomList) {
            var component = CreateComponent<ItemBoardComponent>(CoreComponentIds.ItemBoard);
            component.roomList = newRoomList;
            return AddComponent(CoreComponentIds.ItemBoard, component);
        }

        public Entity ReplaceItemBoard(SingleGrid newRoomList) {
            var component = CreateComponent<ItemBoardComponent>(CoreComponentIds.ItemBoard);
            component.roomList = newRoomList;
            ReplaceComponent(CoreComponentIds.ItemBoard, component);
            return this;
        }

        public Entity RemoveItemBoard() {
            return RemoveComponent(CoreComponentIds.ItemBoard);
        }
    }

    public partial class Pool {

        public Entity itemBoardEntity { get { return GetGroup(CoreMatcher.ItemBoard).GetSingleEntity(); } }
        public ItemBoardComponent itemBoard { get { return itemBoardEntity.itemBoard; } }
        public bool hasItemBoard { get { return itemBoardEntity != null; } }

        public Entity SetItemBoard(SingleGrid newRoomList) {
            if(hasItemBoard) {
                throw new EntitasException("Could not set itemBoard!\n" + this + " already has an entity with ItemBoardComponent!",
                    "You should check if the pool already has a itemBoardEntity before setting it or use pool.ReplaceItemBoard().");
            }
            var entity = CreateEntity();
            entity.AddItemBoard(newRoomList);
            return entity;
        }

        public Entity ReplaceItemBoard(SingleGrid newRoomList) {
            var entity = itemBoardEntity;
            if(entity == null) {
                entity = SetItemBoard(newRoomList);
            } else {
                entity.ReplaceItemBoard(newRoomList);
            }

            return entity;
        }

        public void RemoveItemBoard() {
            DestroyEntity(itemBoardEntity);
        }
    }
}

    public partial class CoreMatcher {

        static IMatcher _matcherItemBoard;

        public static IMatcher ItemBoard {
            get {
                if(_matcherItemBoard == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.ItemBoard);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherItemBoard = matcher;
                }

                return _matcherItemBoard;
            }
        }
    }
