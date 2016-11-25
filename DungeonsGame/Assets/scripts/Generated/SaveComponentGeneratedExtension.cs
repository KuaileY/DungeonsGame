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

        public SaveComponent save { get { return (SaveComponent)GetComponent(InputComponentIds.Save); } }
        public bool hasSave { get { return HasComponent(InputComponentIds.Save); } }

        public Entity AddSave(string newName, System.Xml.Linq.XDocument newXDoc) {
            var component = CreateComponent<SaveComponent>(InputComponentIds.Save);
            component.name = newName;
            component.xDoc = newXDoc;
            return AddComponent(InputComponentIds.Save, component);
        }

        public Entity ReplaceSave(string newName, System.Xml.Linq.XDocument newXDoc) {
            var component = CreateComponent<SaveComponent>(InputComponentIds.Save);
            component.name = newName;
            component.xDoc = newXDoc;
            ReplaceComponent(InputComponentIds.Save, component);
            return this;
        }

        public Entity RemoveSave() {
            return RemoveComponent(InputComponentIds.Save);
        }
    }

    public partial class Pool {

        public Entity saveEntity { get { return GetGroup(InputMatcher.Save).GetSingleEntity(); } }
        public SaveComponent save { get { return saveEntity.save; } }
        public bool hasSave { get { return saveEntity != null; } }

        public Entity SetSave(string newName, System.Xml.Linq.XDocument newXDoc) {
            if(hasSave) {
                throw new EntitasException("Could not set save!\n" + this + " already has an entity with SaveComponent!",
                    "You should check if the pool already has a saveEntity before setting it or use pool.ReplaceSave().");
            }
            var entity = CreateEntity();
            entity.AddSave(newName, newXDoc);
            return entity;
        }

        public Entity ReplaceSave(string newName, System.Xml.Linq.XDocument newXDoc) {
            var entity = saveEntity;
            if(entity == null) {
                entity = SetSave(newName, newXDoc);
            } else {
                entity.ReplaceSave(newName, newXDoc);
            }

            return entity;
        }

        public void RemoveSave() {
            DestroyEntity(saveEntity);
        }
    }
}

    public partial class InputMatcher {

        static IMatcher _matcherSave;

        public static IMatcher Save {
            get {
                if(_matcherSave == null) {
                    var matcher = (Matcher)Matcher.AllOf(InputComponentIds.Save);
                    matcher.componentNames = InputComponentIds.componentNames;
                    _matcherSave = matcher;
                }

                return _matcherSave;
            }
        }
    }
