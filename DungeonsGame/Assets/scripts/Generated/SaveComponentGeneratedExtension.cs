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

        static readonly SaveComponent saveComponent = new SaveComponent();

        public bool isSave {
            get { return HasComponent(InputComponentIds.Save); }
            set {
                if(value != isSave) {
                    if(value) {
                        AddComponent(InputComponentIds.Save, saveComponent);
                    } else {
                        RemoveComponent(InputComponentIds.Save);
                    }
                }
            }
        }

        public Entity IsSave(bool value) {
            isSave = value;
            return this;
        }
    }

    public partial class Pool {

        public Entity saveEntity { get { return GetGroup(InputMatcher.Save).GetSingleEntity(); } }

        public bool isSave {
            get { return saveEntity != null; }
            set {
                var entity = saveEntity;
                if(value != (entity != null)) {
                    if(value) {
                        CreateEntity().isSave = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
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
