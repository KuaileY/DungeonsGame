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

        static readonly NewGameComponent newGameComponent = new NewGameComponent();

        public bool isNewGame {
            get { return HasComponent(InputComponentIds.NewGame); }
            set {
                if(value != isNewGame) {
                    if(value) {
                        AddComponent(InputComponentIds.NewGame, newGameComponent);
                    } else {
                        RemoveComponent(InputComponentIds.NewGame);
                    }
                }
            }
        }

        public Entity IsNewGame(bool value) {
            isNewGame = value;
            return this;
        }
    }

    public partial class Pool {

        public Entity newGameEntity { get { return GetGroup(InputMatcher.NewGame).GetSingleEntity(); } }

        public bool isNewGame {
            get { return newGameEntity != null; }
            set {
                var entity = newGameEntity;
                if(value != (entity != null)) {
                    if(value) {
                        CreateEntity().isNewGame = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }
}

    public partial class InputMatcher {

        static IMatcher _matcherNewGame;

        public static IMatcher NewGame {
            get {
                if(_matcherNewGame == null) {
                    var matcher = (Matcher)Matcher.AllOf(InputComponentIds.NewGame);
                    matcher.componentNames = InputComponentIds.componentNames;
                    _matcherNewGame = matcher;
                }

                return _matcherNewGame;
            }
        }
    }
