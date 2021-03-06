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

        static readonly InteractiveComponent interactiveComponent = new InteractiveComponent();

        public bool isInteractive {
            get { return HasComponent(CoreComponentIds.Interactive); }
            set {
                if(value != isInteractive) {
                    if(value) {
                        AddComponent(CoreComponentIds.Interactive, interactiveComponent);
                    } else {
                        RemoveComponent(CoreComponentIds.Interactive);
                    }
                }
            }
        }

        public Entity IsInteractive(bool value) {
            isInteractive = value;
            return this;
        }
    }
}

    public partial class CoreMatcher {

        static IMatcher _matcherInteractive;

        public static IMatcher Interactive {
            get {
                if(_matcherInteractive == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.Interactive);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherInteractive = matcher;
                }

                return _matcherInteractive;
            }
        }
    }
